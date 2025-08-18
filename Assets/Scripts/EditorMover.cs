using UnityEngine;

namespace DefaultNamespace
{
	
	[RequireComponent(typeof(PositionSaver))]
	public class EditorMover : MonoBehaviour
	{
		private PositionSaver _save;
		private float _currentDelay;

		//todo comment: Что произойдёт, если _delay > _duration?
		// В save.Records может быть добавлено меньше точек либо же ни одной 
		[field: SerializeField, Range(0.2f, 1.0f)]
        private float _delay = 0.5f;
		[field: SerializeField, Min(0.2f)]
		private float _duration = 5f;

		private void Start()
		{
            if (_duration <= _delay)
            {
                _duration = _delay * 5f;
              
            }
            //todo comment: Почему этот поиск производится здесь, а не в начале метода Update?

            // Поиск компонента выполняется в Start, чтобы избежать лишних вызовов GetComponent в каждом кадре
            _save = GetComponent<PositionSaver>();
			_save.Records.Clear();
		}

		private void Update()
		{
			_duration -= Time.deltaTime;
			if (_duration <= 0f)
			{
				enabled = false;
				Debug.Log($"<b>{name}</b> finished", this);
				return;
			}

            //todo comment: Почему не написать (_delay -= Time.deltaTime;) по аналогии с полем _duration?

            //delay это фиксированный интервал между сохранениями, currentDelay это временной счётчик, который отсчитывает время до следующего сохранения
            _currentDelay -= Time.deltaTime;
			if (_currentDelay <= 0f)
			{
				_currentDelay = _delay;
				_save.Records.Add(new PositionSaver.Data
				{
					Position = transform.position,
                    //todo comment: Для чего сохраняется значение игрового времени?
                    // Чтобы знать, в какой момент времени была записана каждая позиция
                    Time = Time.time,
				});
			}
		}
	}
}