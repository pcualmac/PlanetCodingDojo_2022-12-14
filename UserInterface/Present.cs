namespace UserInterface
{
    public enum ProcessState
    {
        NewPresent,
        PackingPresent,
        DropedPresent,
        DeliveredPresent   
    }

    public enum Command
    {
        Start,
        Drop,
        Packing,
        Delivered
    }

    public class Present
    {
        class StateTransition
        {
            readonly ProcessState CurrentState;
            readonly Command Command;

            public StateTransition(ProcessState currentState, Command command)
            {
                CurrentState = currentState;
                Command = command;
            }

            public override int GetHashCode()
            {
                return 17 + 31 * CurrentState.GetHashCode() + 31 * Command.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                StateTransition other = obj as StateTransition;
                return other != null && this.CurrentState == other.CurrentState && this.Command == other.Command;
            }
        }
        Dictionary<StateTransition, ProcessState> transitions;
        public ProcessState CurrentState { get; private set; }
        public string Name { get; }
        public string Owner { get; }
        public Present(string name, string owner="jerry")
        {
            CurrentState = ProcessState.NewPresent;
            transitions = new Dictionary<StateTransition, ProcessState>
            {
                { new StateTransition(ProcessState.NewPresent, Command.Packing), ProcessState.PackingPresent },
                { new StateTransition(ProcessState.PackingPresent, Command.Delivered), ProcessState.DeliveredPresent },
                { new StateTransition(ProcessState.PackingPresent, Command.Drop), ProcessState.DropedPresent },
                { new StateTransition(ProcessState.NewPresent, Command.Drop), ProcessState.DropedPresent },
                { new StateTransition(ProcessState.DeliveredPresent, Command.Drop), ProcessState.DropedPresent },
            };
        }

        public ProcessState GetNext(Command command)
        {
            StateTransition transition = new StateTransition(CurrentState, command);
            ProcessState nextState;
            if (!transitions.TryGetValue(transition, out nextState))
                throw new Exception("Invalid transition: " + CurrentState + " -> " + command);
            return nextState;
        }

        public ProcessState MoveNext(Command command)
        {
            CurrentState = GetNext(command);
            return CurrentState;
        }
    }
}