using System;
using System.Collections.Generic;
using System.Text;

namespace GameOfFifteen
{
    // пріоритетна черга , зберігає стани графу які заповнюються у вигляді черги
    // де члений з найменшим значенням CostH стають на початок черги 
    public class MinPriorityQueue<T> where T : IComparable
    {
        // Даний клас реалізує пріориетну чергу
        // в чергу додаються значення CostF 
        // Якщо значення CostF менше воно додається ближче до початку черги

        // Черга реалізована на масиві і за структурою нагадує бінарне дерево
        // тобто значення нащадків батька(i) розміщуються за індексами 2*i , 2*i+1
        // а значення батька за індексом нащадка(k) знаходиться за індексом k/2
        // Перший елемент черги завжди null


        // Змінна для зберігання членів черги
        private T[] mArray;
        private int mCount;

        // Конструктор
        public MinPriorityQueue(int capacity)
        {
            mArray = new T[capacity + 1];
            mCount = 0;
        }

        // Збільшення розміру черги по значенню capacity
        private void Expand(int capacity)
        {
            T[] temp = new T[capacity + 1];
            int i = 0;
            while (++i <= mCount)
            {
                temp[i] = mArray[i];
                mArray[i] = default(T);
            }

            mArray = temp;
        }

        // Порівнює 2 елементи черги
        private bool Less(int i, int j)
        {
            return mArray[i].CompareTo(mArray[j]) < 0;
        }

        // Свап елементів
        private void Swap(int i, int j)
        {
            T temp = mArray[j];
            mArray[j] = mArray[i];
            mArray[i] = temp;
        }


        // Переформовує Чергу якщо  батько більший за нащадків
        // Якщо CostF батька більше за нащадки то черга переформовується
        //      (CostF = 5)                         (CostF = 2)
        //          /\                                  /\
        //         /  \                ===>            /  \
        //(CostF = 2)  (CostF = 4)             (CostF = 5) (CostF = 4)
        private void Sink(int index)
        {
            int k;
            while (index * 2 <= mCount)
            {
                k = index * 2;

                if (k + 1 <= mCount && Less(k + 1, k))
                {
                    k = k + 1;
                }
                if (!Less(k, index))
                {
                    break;
                }
                Swap(index, k);
                index = k;
            }
        }


        // Переформовує чергу якщо мінімальний член дерева є меншим за одного з нащадків,
        // тобто якщо CostF більше у нащадка ніж у батька(кореня)
        private void Swim(int index)
        {
            int k;

            while (index / 2 > 0)
            {
                k = index / 2;

                if (!Less(index, k))
                {
                    break;
                }

                Swap(index, k);
                index = k;
            }
        }


        // Чи черга пуста?
        public bool IsEmpty()
        {
            return mCount == 0;
        }

        // Добавити елемент в чергу
        // Якщо немає місця збільшити розмір черги втричі
        public void Enqueue(T item)
        {
            if (mCount == mArray.Length - 1)
            {
                Expand(mArray.Length * 3);
            }

            mArray[++mCount] = item;
            Swim(mCount);
        }


        // Видалити елемент з початку черги
        public T Dequeue()
        {
            if (!IsEmpty())
            {
                T item = mArray[1];
                mArray[1] = mArray[mCount];
                mArray[mCount--] = default(T);

                Sink(1);

                return item;
            }

            return default(T);
        }

        // Знайти елемент по значенню та повернути його індекс і значення
        public T Find(T item, out int index)
        {
            index = -1;
            if (!IsEmpty())
            {
                int i = 0;

                while (++i <= mCount)
                {
                    if (mArray[i].Equals(item))
                    {
                        index = i;
                        return mArray[i];
                    }
                }
            }

            return default(T);
        }


        // Видалити елемент з чергий по індексу
        public void Remove(int index)
        {
            if (index > 0 && index <= mCount)
            {
                mArray[index] = mArray[mCount];
                mArray[mCount--] = default(T);
                Sink(index);
            }
        }


    }
}
