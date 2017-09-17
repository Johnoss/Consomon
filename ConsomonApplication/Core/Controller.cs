using System;

namespace ConsomonApplication
{
    [Serializable]
    public class Controller
    {
        public delegate void InputReadEventHandler(object source, KeyReadEventArgs args);
        public EventHandler<KeyReadEventArgs> InputRead;

        public void ReadInput(Player player, bool clear = false)
        {
            while (true)
            {
                ConsoleKey input = Console.ReadKey(true).Key;
                if (player.ControlScheme.ContainsKey(input))
                {
                    OnInputRead(input);
                    return;
                }
            }
        }

        protected virtual void OnInputRead(ConsoleKey inputKey)
        {
            InputRead?.Invoke(this, new KeyReadEventArgs() { InputKey = inputKey });
        }
    }
}
