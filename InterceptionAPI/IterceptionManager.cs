using KeyList = System.Collections.Generic.List<Key>;
public class IterceptionManager
{
    public Dictionary<int, KeyList> DownedKeys = new Dictionary<int, KeyList>();
    public IntPtr Keyboard;
    public int KeyboardDeviceID = 2;
    public IntPtr Mouse;
    public int MouseDeviceID = 13;

    private Thread driverupdaterkeyboard;
    private Thread driverupdatermouse;

    public IterceptionManager()
    {
        Keyboard = Interception.CreateContext();
        Interception.SetFilter(Keyboard, Interception.IsKeyboard, Interception.Filter.All);
        Mouse = Interception.CreateContext();
        Interception.SetFilter(Mouse, Interception.IsMouse, Interception.Filter.All);
    }

    private KeyList GetOrCreateKeyList(Dictionary<int, KeyList> dictionary, int deviceID)
    {
        KeyList result;
        if (!dictionary.TryGetValue(deviceID, out result))
            dictionary[deviceID] = result = new KeyList();
        return result;
    }

    public Key ToKey(Interception.KeyStroke keyStroke)
    {
        var result = keyStroke.Code;
        if ((keyStroke.State & Interception.KeyState.E0) != 0)
            result += 0x100;
        return (Key)result;
    }

    public Interception.KeyStroke ToKeyStroke(Key key, bool down)
    {
        var result = new Interception.KeyStroke();
        if (!down)
            result.State = Interception.KeyState.Up;
        var code = (short) key;
        if (code >= 0x100)
        {
            code -= 0x100;
            result.State |= Interception.KeyState.E0;
        }
        else if (code < 0)
        {
            code += 100;
            result.State |= Interception.KeyState.E0;
        }
        result.Code = (ushort)code;
        return result;
    }

    public void Quit()
    {
        Interception.DestroyContext(Keyboard);
        Interception.DestroyContext(Mouse);

        try { driverupdaterkeyboard.Interrupt(); } catch { }
        try { driverupdatermouse.Interrupt(); } catch { }        
    }

    private void QuitWithException(Exception ex)
    {
        Console.WriteLine($"Thrown exception at IterceptionManager: \n{ex}");

        Quit();
    }

    public void Start()
    {
        if (driverupdaterkeyboard == null)
        {
            driverupdaterkeyboard = new Thread(DriverKUpdater);
            driverupdaterkeyboard.Priority = ThreadPriority.Highest;
            driverupdaterkeyboard.Start();
        }
        if (driverupdatermouse == null)
        {
            driverupdatermouse = new Thread(DriverMUpdater);
            driverupdatermouse.Priority = ThreadPriority.Highest;
            driverupdatermouse.Start();
        }
    }

    private void DriverKUpdater()
    {
        try
        {
            while (true)
            {
                DriverUpdaterKeyBoard();
                Thread.Sleep(1);
            }
                
        }
        catch (Exception ex)
        {
            QuitWithException(ex);
        }
    }

    private void DriverMUpdater()
    {
        try
        {
            while (true)
            {
                DriverUpdaterMouse();
                Thread.Sleep(1);
            }
        }
        catch (Exception ex)
        {
            QuitWithException(ex);
        }

    }

    public void DriverUpdaterMouse()
    {
        var mousedeviceID = Interception.WaitWithTimeout(Mouse, 0);
        if (mousedeviceID == 0)
            return;

        var stroke = new Interception.Stroke();
        while (Interception.Receive(Mouse, mousedeviceID, ref stroke, 1) > 0)
        {
            var processed = false;
            switch (stroke.Mouse.State)
            {
                case Interception.MouseState.LeftButtonDown:
                    processed = InternalOnKeyDown(Key.MouseLeft, false);
                    break;
                case Interception.MouseState.RightButtonDown:
                    processed = InternalOnKeyDown(Key.MouseRight, false);
                    break;
                case Interception.MouseState.MiddleButtonDown:
                    processed = InternalOnKeyDown(Key.MouseMiddle, false);
                    break;
                case Interception.MouseState.Button4Down:
                    processed = InternalOnKeyDown(Key.Button1, false);
                    break;
                case Interception.MouseState.Button5Down:
                    processed = InternalOnKeyDown(Key.Button2, false);
                    break;
                case Interception.MouseState.LeftButtonUp:
                    processed = InternalOnKeyUp(Key.MouseLeft);
                    break;
                case Interception.MouseState.RightButtonUp:
                    processed = InternalOnKeyUp(Key.MouseRight);
                    break;
                case Interception.MouseState.MiddleButtonUp:
                    processed = InternalOnKeyUp(Key.MouseMiddle);
                    break;
                case Interception.MouseState.Button4Up:
                    processed = InternalOnKeyUp(Key.Button1);
                    break;
                case Interception.MouseState.Button5Up:
                    processed = InternalOnKeyUp(Key.Button2);
                    break;
                case Interception.MouseState.Wheel:
                    processed = InternalOnMouseWheel(stroke.Mouse.Rolling);
                    break;
            }
            processed = InternalOnMouseMove(stroke.Mouse.X, stroke.Mouse.Y);
            if (!processed)
                Interception.Send(Mouse, mousedeviceID, ref stroke, 1);
        }
    }

    public void DriverUpdaterKeyBoard()
    {
        var keyboardDeviceID = Interception.WaitWithTimeout(Keyboard, 0);
        if (keyboardDeviceID == 0)
            return;

        var stroke = new Interception.Stroke();
        while (Interception.Receive(Keyboard, keyboardDeviceID, ref stroke, 1) > 0)
        {
            var key = ToKey(stroke.Key);
            var processed = false;
            var deviceDownedKeys = GetOrCreateKeyList(DownedKeys, keyboardDeviceID);
            switch (stroke.Key.State.IsKeyDown())
            {
                case true:
                    switch (!deviceDownedKeys.Contains(key))
                    {
                        case true:
                            deviceDownedKeys.Add(key);
                            processed = InternalOnKeyDown(key, false);
                            break;
                        case false:
                            processed = InternalOnKeyDown(key, true);
                            break;
                    }
                    break;
                case false:
                    deviceDownedKeys.Remove(key);
                    processed = InternalOnKeyUp(key);
                    break;
            }
            if (!processed)
                Interception.Send(Keyboard, keyboardDeviceID, ref stroke, 1);
        }
    }

    public void Stop()
    {
        driverupdaterkeyboard.Interrupt();
        driverupdatermouse.Interrupt();
    }

    public delegate bool OnMouseMoveHandler(int x, int y);
    public event OnMouseMoveHandler? OnMouseMove;

    public delegate bool OnMouseWheelHandler(int rolling);
    public event OnMouseWheelHandler? OnMouseWheel;

    public delegate bool OnKeyDownHandler(Key key, bool repeat);
    public event OnKeyDownHandler? OnKeyDown;

    public delegate bool OnKeyUpHandler(Key key);
    public event OnKeyUpHandler? OnKeyUp;

	private bool InternalOnMouseMove(int x, int y)
    {
        try 
        { 
            if (OnMouseMove != null)
                return OnMouseMove(x, y); 
        } catch {}
        return false;
    }

    private bool InternalOnMouseWheel(int rolling)
    {
		try
		{
			if (OnMouseWheel != null)
                return OnMouseWheel(rolling);
		}
		catch {}
		return false;
    }

    private bool InternalOnKeyDown(Key key, bool repeat)
    {
		try
		{
			if (OnKeyDown != null)
                return OnKeyDown(key, repeat);
		}
		catch {}
		return false;
    }

    private bool InternalOnKeyUp(Key key)
    {
		try
		{
			if (OnKeyUp != null)
                return OnKeyUp(key);
		}
		catch {}
		return false;
	}

	#region Macros
	public void Sleep(float delay) => Thread.Sleep(new TimeSpan(0, 0, 0, (int)delay, (int)(delay % 1)));

    public void KeyClick(Key key, float delay)
    {
        KeyDown(key);
        Sleep(delay);
        KeyUp(key);
    }

    public bool IsKeyDown(int deviceID, Key key)
    {
        KeyList deviceDownedKeys;
        if (!DownedKeys.TryGetValue(deviceID, out deviceDownedKeys))
            return false;
        return deviceDownedKeys.Contains(key);
    }

    public bool IsKeyDown(Key key) => IsKeyDown(KeyboardDeviceID, key);

    public bool IsKeyUp(int deviceID, Key key) => !IsKeyDown(deviceID, key);

    public bool IsKeyUp(Key key) => IsKeyUp(KeyboardDeviceID, key);

    public void KeyDown(int deviceID, params Key[] keys)
    {
        foreach (var key in keys)
        {
            if (((short)key) < 0)
            {
                var stroke = new Interception.Stroke();
                switch (key)
                {
                    case Key.MouseLeft:
                        stroke.Mouse.State = Interception.MouseState.LeftButtonUp;
                        break;
                    case Key.MouseRight:
                        stroke.Mouse.State = Interception.MouseState.RightButtonUp;
                        break;
                    case Key.MouseMiddle:
                        stroke.Mouse.State = Interception.MouseState.MiddleButtonUp;
                        break;
                    case Key.Button1:
                        stroke.Mouse.State = Interception.MouseState.Button4Up;
                        break;
                    case Key.Button2:
                        stroke.Mouse.State = Interception.MouseState.Button5Up;
                        break;
                }
                Interception.Send(Mouse, MouseDeviceID, ref stroke, 1);
            }
            else
            {
                var stroke = new Interception.Stroke();
                stroke.Key = ToKeyStroke(key, true);
                Interception.Send(Keyboard, deviceID, ref stroke, 1);
            }
        }
    }

    public void KeyDown(params Key[] keys) => KeyDown(KeyboardDeviceID, keys);

    public void KeyUp(int deviceID, params Key[] keys)
    {
        foreach (var key in keys)
        {
            if (((short)key) < 0)
            {
                var stroke = new Interception.Stroke();
                switch (key)
                {
                    case Key.MouseLeft:
                        stroke.Mouse.State = Interception.MouseState.LeftButtonDown;
                        break;
                    case Key.MouseRight:
                        stroke.Mouse.State = Interception.MouseState.RightButtonDown;
                        break;
                    case Key.MouseMiddle:
                        stroke.Mouse.State = Interception.MouseState.MiddleButtonDown;
                        break;
                    case Key.Button1:
                        stroke.Mouse.State = Interception.MouseState.Button4Down;
                        break;
                    case Key.Button2:
                        stroke.Mouse.State = Interception.MouseState.Button5Down;
                        break;
                }
                Interception.Send(Mouse, MouseDeviceID, ref stroke, 1);
            }
            else
            {
                var stroke = new Interception.Stroke();
                stroke.Key = ToKeyStroke(key, false);
                Interception.Send(Keyboard, deviceID, ref stroke, 1);
            }
        }
    }

    public void KeyUp(params Key[] keys) => KeyUp(KeyboardDeviceID, keys);

    public void MouseScroll(int deviceID, short rolling)
    {
        var stroke = new Interception.Stroke();
        stroke.Mouse.State = Interception.MouseState.Wheel;
        stroke.Mouse.Rolling = rolling;
        Interception.Send(Mouse, deviceID, ref stroke, 1);
    }

    public void MouseScroll(short rolling) => MouseScroll(MouseDeviceID, rolling);

    public void MouseMove(int deviceID, int x, int y)
    {
        var stroke = new Interception.Stroke();
        stroke.Mouse.X = x;
        stroke.Mouse.Y = y;
        stroke.Mouse.Flags = Interception.MouseFlag.MoveRelative;
        Interception.Send(Mouse, deviceID, ref stroke, 1);
    }

    public void MouseSet(int deviceID, int x, int y)
    {
        var stroke = new Interception.Stroke();
        stroke.Mouse.X = x;
        stroke.Mouse.Y = y;
        stroke.Mouse.Flags = Interception.MouseFlag.MoveAbsolute;
        Interception.Send(Mouse, deviceID, ref stroke, 1);
    }

    public void MouseMove(int x, int y) => MouseMove(MouseDeviceID, x, y);

    public void MouseSet(int x, int y) => MouseSet(MouseDeviceID, x, y);
    #endregion
}