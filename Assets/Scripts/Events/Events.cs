    using EdwinGameDev.Gameplay;

    namespace EdwinGameDev.EventSystem
    {
        public class Events
        {
            public class GameStarted
            {
            
            }
        
            public class GameEnded
            {
            
            }
        
            public class HoleMoved
            {
            
            }
        
            public class HoleCreated
            {
                public Hole Hole;

                public HoleCreated(Hole hole)
                {
                    Hole = hole;
                }
            }

            public class OnMatchTimerUpdated
            {
                public readonly float Time;

                public OnMatchTimerUpdated(float time)
                {
                    Time = time;
                }
            }
        }
    }
