using System;
using UnityEngine;

namespace DefaultNamespace
{
	[RequireComponent(typeof(PositionSaver))]
	public class ReplayMover : MonoBehaviour
	{
		private PositionSaver _save;

		private int _index;
		private PositionSaver.Data _prev;
		private float _duration;

		private void Start()
		{
            //todo comment: зачем нужны эти проверки?
            /// Проверки гарантируют, что компонент имеет все необходимые данные для работы
            if (!TryGetComponent(out _save) || _save.Records.Count == 0)
			{
				Debug.LogError("Records incorrect value", this);
                //todo comment: Для чего выключается этот компонент?
                /// Не тратить ресурсы на бесполезные вычисления в Update() и избежать ошибок

                enabled = false;
			}
		}

		private void Update()
		{
			var curr = _save.Records[_index];
            //todo comment: Что проверяет это условие (с какой целью)? 
            /// С целью нужно ли переключиться на следующую запись в Records
            if (Time.time > curr.Time)
			{
				_prev = curr;
				_index++;
                //todo comment: Для чего нужна эта проверка?
                /// проверяет, достигли ли конца списка записей
                if (_index >= _save.Records.Count)
				{
					enabled = false;
					Debug.Log($"<b>{name}</b> finished", this);
				}
			}
            //todo comment: Для чего производятся эти вычисления (как в дальнейшем они применяются)?
            /// Формула вычисляет время delta между двумя точками. Променяется для интерполяции движения
            var delta = (Time.time - _prev.Time) / (curr.Time - _prev.Time);
            //todo comment: Зачем нужна эта проверка?
            /// float.IsNaN(delta) возвращает true, если delta не является число (деление на ноль)
            if (float.IsNaN(delta)) delta = 0f;
            //todo comment: Опишите, что происходит в этой строчке так подробно, насколько это возможно
            /// строка плавно перемещает объект между двумя точками: _prev.Position - начальная точка, curr.Position - конечная точка, delta - интерполяционный коэффициент.
			transform.position = Vector3.Lerp(_prev.Position, curr.Position, delta);
		}
	}
}