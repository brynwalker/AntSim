﻿namespace MyGame
{
    public class Ant : Creature
    {
        private readonly Nest _nest;

        private bool _wander;
        private bool _return;
        private bool _getFood;

        private int _food;
        private readonly int _maxFood;

        public Ant(Nest n)
            :base(new Location(n.Location))
        {
            _nest = n;

            _wander = false;
            _return = false;
            _getFood = true;

            _maxFood = 1;
        }

        public override void Move()
        {
            foreach (Food f in GameLogic.Foods)
            {
                if (f.CheckCollision(Location) && _food < _maxFood)
                {
                    if (f.Size == 0)
                    {
                        _return = true;
                        _getFood = false;
                    }
                    else
                    {
                        //Console.WriteLine("Taking food!");
                        _food = f.TakeFood(1);
                    }
                }
            }

            if (_food == _maxFood)
            {
                _return = true;
                _wander = false;
                _getFood = false;
            }

            if (_nest.CheckCollision(Location))
            {
                _nest.AddFood(_food);
                _food = 0;
                _getFood = true;
                _return = false;
                CurrentPath = null;
            }

            if (_wander)
            {
                if (CurrentPath == null || Location.IsAt(CurrentPath.Destination))
                    CurrentPath = Wander();

                base.Move();
            }
            else if (_getFood)
            {
                if (CurrentPath == null || Location.IsAt(CurrentPath.Destination))
                    CurrentPath = PathfindToRandomFood();
            }
            else if (_return)
            {
                CurrentPath = GetPathTo(_nest.Location);
            }

            base.Move();
        }

        public Path PathfindToRandomFood()
        {
            Food targetFood = GetRandomFood();
            Location destination = new Location(targetFood.Location);
            return GetPathTo(destination);
        }

        public Food GetRandomFood()
        {
            Food bestFood = null;
            int bestScore = 0;

            foreach (Food f in GameLogic.Foods)
            {
                if (100 + f.Size + GameLogic.Random.Next(50) -
                    PathingUtils.GetFScore(f.Location, new Node(Location.X, Location.Y)) > bestScore)
                {
                    bestFood = f;
                    bestScore = f.Size;
                }
            }

            return bestFood;
        }

        public void CheckForFood()
        {
            foreach (Food f in GameLogic.Foods)
            {
                CurrentPath = GetPathTo(f.Location);
                _wander = false;
                _getFood = true;
                return;
            }
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

        public int Food
        {
            get { return _food; }
        }
    }
}