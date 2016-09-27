﻿namespace MyGame
{
    public class Ant : Creature
    {
        private Nest _nest;
        private bool _wander;
        private bool _return;
        private bool _getFood;

        public Ant(Nest n)
            : base(new Location(n.Location))
        {
            _nest = n;

            // Default pathfinding state is wandering.
            _wander = true;
            _return = false;
            _getFood = false;
        }

        public override void Move()
        {
            if (_wander)
            {
                if (CurrentPath == null || (Location.X == CurrentPath.Destination.X
                                            && Location.Y == CurrentPath.Destination.Y))
                    CurrentPath = Wander();

                base.Move();
            }
            else if (_getFood)
                base.Move();
        }

        public void CheckForFood()
        {
            foreach (Food f in GameLogic.Food)
            {
                if (FoodProximity(f))
                {
                    CurrentPath = GetPathTo(f.Location);
                    _wander = false;
                    _getFood = true;
                    return;
                }
            }
        }

        private bool FoodProximity(Food f)
        {
            return (f.Location.X - Location.X < 300 && f.Location.X - Location.X > -300 )
                    && (f.Location.Y - Location.Y < 300 && f.Location.Y - Location.Y > -300);
        }


        public Nest Nest
        {
            get { return _nest; }
        }

        public bool Wandering
        {
            get { return _wander; }
            set { _wander = value; }
        }

        public bool Returning
        {
            get { return _return; }
            set { _return = value; }
        }

        public bool GetFood
        {
            get { return _getFood; }
            set { _getFood = value; }
        }
    }
}