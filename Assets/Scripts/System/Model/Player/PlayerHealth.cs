using UniRx;

namespace System.Model
{
    public class PlayerHealth
    {
        public ReactiveProperty<float> Health;
        public PlayerHealth(float health = 100) => Health = new ReactiveProperty<float>(health);
    }
}