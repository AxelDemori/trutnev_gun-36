using UniRx;

namespace System.Model
{
    public class BonusCount
    {
        public ReactiveProperty<int> Count = new IntReactiveProperty();
    }
}