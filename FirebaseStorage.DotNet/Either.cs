namespace GameCtor.FirebaseStorage.DotNet
{
    public abstract class Either<TLeft, TRight>
    {
        public abstract bool IsLeft { get; }
        public bool IsRight { get { return !IsLeft; } }
        public abstract TLeft Left { get; }
        public abstract TRight Right { get; }
    }

    public static class Either
    {
        public sealed class LeftValue<TLeft, TRight> : Either<TLeft, TRight>
        {
            TLeft _leftValue;

            public LeftValue(TLeft leftValue)
            {
                _leftValue = leftValue;
            }

            public override TLeft Left { get { return _leftValue; } }
            public override TRight Right { get { return default(TRight); } }
            public override bool IsLeft { get { return true; } }
        }

        public sealed class RightValue<TLeft, TRight> : Either<TLeft, TRight>
        {
            TRight _rightValue;

            public RightValue(TRight rightValue)
            {
                _rightValue = rightValue;
            }

            public override TLeft Left { get { return default(TLeft); } }
            public override TRight Right { get { return _rightValue; } }
            public override bool IsLeft { get { return false; } }
        }

        // Factory functions to create left or right-valued Either instances
        public static Either<TLeft, TRight> Left<TLeft, TRight>(TLeft leftValue)
        {
            return new LeftValue<TLeft, TRight>(leftValue);
        }

        public static Either<TLeft, TRight> Right<TLeft, TRight>(TRight rightValue)
        {
            return new RightValue<TLeft, TRight>(rightValue);
        }
    }
}
