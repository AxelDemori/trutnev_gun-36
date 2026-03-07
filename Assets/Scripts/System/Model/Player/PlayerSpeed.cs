using UniRx;

namespace System.Model
{
    public class PlayerSpeed
    {
        public ReactiveProperty<float> Speed;

        public PlayerSpeed(float speed) => Speed = new ReactiveProperty<float>(speed);
    }
}