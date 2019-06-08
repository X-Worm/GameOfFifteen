using System;
using System.Collections.Generic;
using System.Text;

namespace GameOfFifteen
{
    public class State : IComparable
    {
        private int[] Nodes; //Список вузлів
        private int SpaceIndex;
        private string StateCode;
        private int CostF; // Сума значень еврістичних функцій
        private int CostH; // Значення еврістичної функції HammingDistance
        private int CostG; // Значення еврістичної функції
        private State Parent; // Попередній стан


        // Конструктор
        public State(State parent, int[] nodes)
        {
            Nodes = nodes;
            Parent = parent;
            CalculateCost(); // Обчислити значення евірстичної функції
            StateCode = GenerateStateCode(); // Отримати представлення в string
        }

        // Функція визначення кращого стану
        public bool IsCostlierThan(State thatState)
        {
            return CostG > thatState.CostG;
        }

        // Повертає строкове представлення
        public string GetStateCode()
        {
            return StateCode;
        }

        // Повертає масив сформований з StateCode
        public int[] GetStateCodeArr()
        {
            string[] str = StateCode.Split(' ');
            int[] result = new int[16];
            for (int i = 0; i < 16; i++)
            {
                result[i] = Convert.ToInt32(str[i]);
            }
            return result;
        }

        // Обчислення ціни (ваги - COST)
        private void CalculateCost()
        {
            if (Parent == null)
            {
                // Якщо Parent == null немає станів , отже CostG = 0
                CostG = 0;
            }
            else
            {
                //Якщо існує попередній state то інкрементуємо CostG
                 //CostG = Parent.CostG + 1;
                CostG = 0;
            }

            // Обраховуємо еврістичне значення HammingDistance
            CostH = GetHeuristicCost();

            CostF = CostG + CostH;
        }

        private int GetHeuristicCost()
        {

            int heuristicCost = 0;
            // еврістична функція HammingDistance визначається як
            // сума клітинок які знаходяться на не своїх місцях
            // Чим це значення менше тим ми ближчі до розвязання задачі

            for (int i = 0; i < Nodes.Length; i++)
            {
                int value = Nodes[i] - 1;
                // Пуста клітинка має значення -1
                // Потрібно декрементувати кожне значення 
                // Оскільки масив починається з індексу 0 а перша клітинка з значенням 1

                // Якщо value = -2 ми знайшли пусту клітинку
                if (value == -2)
                {
                    value = Nodes.Length - 1;
                    SpaceIndex = i; // Запамятовуєм положення пустої клітинки
                }

                if (value != i)
                {
                    // Якщо клітинка не пуста і не на свому місці то інкрементуєм CostH
                    heuristicCost++;
                }
            }

            return heuristicCost;
        }

        // Сформувати поточний стан в вигляді строки
        private string GenerateStateCode()
        {
            return string.Join(" ", Nodes);
        }

        // Сформувати поточний стан у вигляді масиву
        public int[] GetState()
        {
            int[] state = new int[Nodes.Length];
            Array.Copy(Nodes, state, Nodes.Length);
            return state;
        }

        public bool IsFinalState()
        {
            // Якщо всі клітинки на своїх місцях то  CostH = 0 і ми у правильній позиції
            return CostH == 0;
        }

        public State GetParent()
        {
            return Parent;
        }

        public List<State> GetNextStates(ref List<State> nextStates)
        {
            nextStates.Clear();
            State state;

            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                state = GetNextState(direction);
                if (state != null)
                {
                    nextStates.Add(state);
                }
            }
            return nextStates;
        }

        private State GetNextState(Direction direction)
        {
            if (CanMove(direction, out int position))
            {
                int[] nodes = new int[Nodes.Length];
                Array.Copy(Nodes, nodes, Nodes.Length);

                Swap(nodes, SpaceIndex, position);

                return new State(this, nodes);
            }
            return null;
        }

        // Swap елементів в вузлах
        private void Swap(int[] nodes, int i, int j)
        {
            int temp = nodes[i];
            nodes[i] = nodes[j];
            nodes[j] = temp;
        }

        // Функція для визначення чи може пуста клітинка переходити до одного з напрямів
        private bool CanMove(Direction direction, out int newPosition)
        {
            int newX = -1; // для збереження координати переходу по X
            int newY = -1; // для збереження координати переходу по Y
            int gridX = (int)Math.Sqrt(Nodes.Length); // Довжина сітки
            int currentX = SpaceIndex % gridX; // Визначення позиції X
            int currentY = SpaceIndex / gridX; // Визначення позиції Y

            //     0  1  2  3
            //    ----- X ----->
            // 0 | 0  1  2  3
            // 1 | 4  5  6  7
            // 2 | 8  9  10 11
            // 3 | 12 13 14 15
            //   v
            //   Y

            newPosition = -1;

            switch (direction)
            {
                case Direction.Up:
                    {
                        // Не можна рухатись вгору якщо CurrentY = 0
                        if (currentY != 0)
                        {
                            newX = currentX;
                            newY = currentY - 1;
                        }
                        break;
                    }

                case Direction.Down:
                    {
                        // Не можна рухатись вних якщо CurrentY більше значення 3
                        if (currentY < (gridX - 1))
                        {
                            newX = currentX;
                            newY = currentY + 1;
                        }
                    }
                    break;

                case Direction.Left:
                    {
                        // Не можна рухатись ліворуч якщо CurrentX = 0
                        if (currentX != 0)
                        {
                            newX = currentX - 1;
                            newY = currentY;
                        }
                        break;
                    }

                case Direction.Right:
                    {
                        // Не можна рухатись праворуч якщо CurrentX пеевищує значення 3
                        if (currentX < (gridX - 1))
                        {
                            newX = currentX + 1;
                            newY = currentY;
                        }
                        break;
                    }

            }

            if (newX != -1 && newY != -1)
            {
                newPosition = newY * gridX + newX; // Формування нової позиції для свапу
            }

            return newPosition != -1;
        }

        public override int GetHashCode()
        {
            return StateCode.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return StateCode.Equals((obj as State).StateCode);
        }
        public int CompareTo(object obj)
        {
            return CostF.CompareTo((obj as State).CostF);
        }
        public override string ToString()
        {
            return $"State: {StateCode}, g: {CostG}, h: {CostH}, f: {CostF}";
        }

    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
    }
}
