using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace GameOfFifteen
{
    public class AstarAlgorithm
    {
        public Stack<State> Run(int[] nodes, ref List<string> AllMoves)
        {
            // Список станів , нащадків батька , Пусту клітинку можна рухита вверх вниз праворуч і ліворуч
            // Даний список зберігає список цих станів з обчисленим значенням CostF
            List<State> nextStates = new List<State>();

            // Список пройдених станів
            HashSet<string> openStates = new HashSet<string>();

            // Список станів які треба пройти
            MinPriorityQueue<State> openedQueue = new MinPriorityQueue<State>(nodes.Length);

            // Список станів які пройдені
            Dictionary<string, State> closedQueue = new Dictionary<string, State>();

            State state = new State(parent: null, nodes: nodes);
            openedQueue.Enqueue(state);
            openStates.Add(state.GetStateCode());


            while (!openedQueue.IsEmpty())
            {
                State currentState = openedQueue.Dequeue();
                openStates.Remove(currentState.GetStateCode());

                // Якщо це правильний стан топовернути значення всіх попередніх станів
                if (currentState.IsFinalState())
                {
                    return GetFinalPath(currentState, ref AllMoves);
                }

                // Отримати всі наступні стани
                currentState.GetNextStates(ref nextStates);

                if (nextStates.Count > 0)
                {
                    State closedState;
                    State openState;
                    State nextState;

                    for (int i = 0; i < nextStates.Count; i++)
                    {
                        closedState = null;
                        openState = null;
                        nextState = nextStates[i];

                        if (openStates.Contains(nextState.GetStateCode()))
                        {
                            // Ми вже маєм такий стан у черзі
                            openState = openedQueue.Find(nextState, out int openStateIndex);

                            if (openState.IsCostlierThan(nextState))
                            {
                                openedQueue.Remove(openStateIndex);
                                openedQueue.Enqueue(nextState);
                            }
                        }
                        else
                        {
                            // Перевірити чи поточний стан є у закритому списку
                            string stateCode = nextState.GetStateCode();

                            if (closedQueue.TryGetValue(stateCode, out closedState))
                            {

                                if (closedState.IsCostlierThan(nextState))
                                {
                                    closedQueue.Remove(stateCode);
                                    closedQueue[stateCode] = nextState;
                                }
                            }
                        }

                        // Якщо отримали новий стан або кращий за попередній
                        if (openState == null && closedState == null)
                        {
                            openedQueue.Enqueue(nextState);
                            openStates.Add(nextState.GetStateCode());
                        }
                    }

                    closedQueue[currentState.GetStateCode()] = currentState;
                }
            }
            // якщо немає результату повернути null
            return null;
        }

        // отримати кінцевий результат
        private Stack<State> GetFinalPath(State state, ref List<string> AllMoves)
        {
            Stack<State> path = new Stack<State>();
            while (state != null)
            {
                path.Push(state);
                state = state.GetParent();
            }
            AllMoves = MakeSolvePath(path);
            return path;
        }

        // Сформувати послідовність дій і записати в текстовий документ
        private List<string> MakeSolvePath(Stack<State> path)
        {
            List<string> AllMoves = new List<string>();
            StringBuilder temp = new StringBuilder(4);

            State FirstState = path.Pop();
            int SpaceIndexFirst = 0, SpaceIndexSecond = 0, ToMove;
            int[] firstArr, secondArr;
            while (path.Count > 0)
            {
                firstArr = FirstState.GetStateCodeArr();
                FirstState = path.Pop();
                secondArr = FirstState.GetStateCodeArr();

                for (int j = 0; j < 16; j++)
                {
                    if (firstArr[j] == -1) SpaceIndexFirst = j;
                    if (secondArr[j] == -1) SpaceIndexSecond = j;
                }
                ToMove = secondArr[SpaceIndexFirst];
                temp.Append(ToMove.ToString() + ",");

                ToMove = SpaceIndexFirst - SpaceIndexSecond;
                switch (ToMove)
                {
                    case -1:
                        {
                            temp.Append(DirectionSymbol.Left);
                            break;
                        }
                    case 1:
                        {
                            temp.Append(DirectionSymbol.Right);
                            break;
                        }
                    case 4:
                        {
                            temp.Append(DirectionSymbol.Down);
                            break;
                        }
                    case -4:
                        {
                            temp.Append(DirectionSymbol.Up);
                            break;
                        }
                }

                AllMoves.Add(temp.ToString());
                temp.Clear();

            }

            StreamWriter str = new StreamWriter("Solver.txt");
            for (int i = 0; i < AllMoves.Count; i++)
            {
                str.WriteLine(AllMoves[i]);
            }
            str.Close();
            return AllMoves;

        }


    }

    public static class DirectionSymbol
    {
        public static char Up = '\u2191';
        public static char Down = '\u2193';
        public static char Left = '\u2190';
        public static char Right = '\u2192';
    }
}
