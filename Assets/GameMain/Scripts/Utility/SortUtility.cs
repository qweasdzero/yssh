namespace StarForce
{
    public static class SortUtility
    {
        /// <summary>
        /// 冒泡排序 --- 优选
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static int[] BubbleSort(int[] array, int count)
        {
            //设置标识符，如果为false意为当前数组为有序，不需要再排序了
            bool shouldSorted = true;

            for (int i = 0; i < count && shouldSorted; i++)
            {
                shouldSorted = false;

                for (int j = count - 1; j > i; j--)
                {
                    if (array[j - 1] > array[j])
                    {
                        shouldSorted = true;
                        XorSwap(ref array[j - 1], ref array[j]);
                    }
                }
            }

            return array;
        }

        /// <summary>
        /// 插入排序 --- 优选
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static int[] InserSort(int[] array, int count)
        {
            int guard; //哨兵，用于暂存需要交换的值

            for (int i = 0; i < count - 1; i++)
            {
                if (array[i] > array[i + 1])
                {
                    guard = array[i + 1];
                    int j;

                    for (j = i; array[j] > guard && j >= 0; j--)
                    {
                        array[j + 1] = array[j]; //赋值操作（依次后移）
                    }

                    array[j + 1] = guard;
                }
            }

            return array;
        }
        //
        // /// <summary>
        // /// 选择排序
        // /// </summary>
        // /// <param name="array"></param>
        // /// <param name="count"></param>
        // /// <returns></returns>
        // private int[] SelectSort(int[] array, int count)
        // {
        //     int min;
        //
        //     for (int i = 0; i < count - 1; i++)
        //     {
        //         min = i;
        //
        //         for (int j = i + 1; j < count; j++)
        //         {
        //             if (array[min] > array[j])
        //             {
        //                 min = j;
        //             }
        //         }
        //
        //         if (min != i)
        //         {
        //             XorSwap(ref array[min], ref array[i]);
        //         }
        //     }
        //
        //     return array;
        // }
        //
        // /// <summary>
        // /// 希尔排序
        // /// </summary>
        // /// <param name="array"></param>
        // /// <param name="count"></param>
        // private int[] ShellSort(int[] array, int count)
        // {
        //     int i, j, guard;
        //     int increment = count;
        //
        //     do
        //     {
        //         increment = increment / 3 + 1; //增量序列
        //
        //         for (i = increment + 1; i < count; i++)
        //         {
        //             if (array[i] < array[i - increment])
        //             {
        //                 guard = array[i]; //暂存在哨兵处
        //
        //                 for (j = i - increment; j >= 0 && guard < array[j]; j -= increment)
        //                 {
        //                     array[j + increment] = array[j]; //记录后移，查找插入位置
        //                 }
        //
        //                 array[j + increment] = guard; //插入
        //             }
        //         }
        //     }
        //     while (increment > 1);
        //
        //     return array;
        // }
        //
        // /// <summary>
        // /// 堆排序_主函数
        // /// </summary>
        // /// <param name="array"></param>
        // /// <param name="count"></param>
        // private void HeapSort(int[] array, int count)
        // {
        //     for (int i = count / 2 - 1; i >= 0; i--) //把array构建成一个大顶堆
        //     {
        //         HeapAdjust(array, i, count - 1);
        //     }
        //
        //     for (int i = count - 1; i > 0; i--)
        //     {
        //         XorSwap(ref array[0], ref array[i]); //将堆顶记录和当前未经排序子序列的最后一个记录交换
        //         HeapAdjust(array, 0, i - 1); //将array[0...i-1]重新调整为大顶堆
        //     }
        // }
        //
        // /// <summary>
        // /// 堆排序_构造大顶堆函数
        // /// </summary>
        // /// <param name="array"></param>
        // /// <param name="startIndex"></param>
        // /// <param name="endIndex"></param>
        // private static void HeapAdjust(int[] array, int startIndex, int endIndex)
        // {
        //     int temp;
        //     temp = array[startIndex];
        //
        //     for (int i = 2 * startIndex + 1; i <= endIndex; i = i * 2 + 1) //沿关键字较大的孩子结点向下筛选
        //     {
        //         if (i < endIndex && array[i] < array[i + 1])
        //         {
        //             ++i; //i为关键字中较大记录的下标
        //         }
        //
        //         if (temp > array[i])
        //         {
        //             break; //rc应插入在位置s上
        //         }
        //
        //         array[startIndex] = array[i];
        //         startIndex = i;
        //     }
        //
        //     array[startIndex] = temp; //插入
        // }
        //
        // /// <summary>
        // /// 归并排序_主函数
        // /// </summary>
        // /// <param name="array"></param>
        // /// <param name="count"></param>
        // private void MergeSort(int[] array, int count)
        // {
        //     int[] tempArray = new int[array.Length]; //申请额外空间，存放归并结果
        //     int k = 1;
        //
        //     while (k < count)
        //     {
        //         MergePass(array, tempArray, k, count); //array归并到tempArray
        //         k = 2 * k; //子序列长度加倍
        //         MergePass(tempArray, array, k, count); //tempArray归并到array
        //         k = 2 * k; //子序列长度加倍
        //     }
        // }
        //
        // /// <summary>
        // /// 归并操作，把SR[]中相邻长度为s的子序列两两归并到TR[]
        // /// </summary>
        // /// <param name="sr"></param>
        // /// <param name="tr"></param>
        // /// <param name="srChildLength"></param>
        // /// <param name="arrayLength"></param>
        // private static void MergePass(int[] sr, int[] tr, int srChildLength, int arrayLength)
        // {
        //     int hasMergeCount = 1; //hasMargeCount代表当前已经归并的元素个数
        //
        //     while (arrayLength - hasMergeCount + 1 >= 2 * srChildLength) //确保此次两两归并可以完成
        //     {
        //         Merge(sr, tr, hasMergeCount - 1, hasMergeCount + srChildLength - 2,
        //             hasMergeCount + 2 * srChildLength - 2); //两两归并
        //
        //         hasMergeCount += 2 * srChildLength;
        //     }
        //
        //     if (arrayLength - hasMergeCount + 1 > srChildLength) //归并最后两个序列
        //     {
        //         Merge(sr, tr, hasMergeCount - 1, hasMergeCount + srChildLength - 2, arrayLength - 1);
        //     }
        //     else //若最后只剩下单个子序列
        //     {
        //         for (int j = hasMergeCount - 1; j < arrayLength; j++)
        //         {
        //             tr[j] = sr[j];
        //         }
        //     }
        // }
        //
        // /// <summary>
        // /// 归并操作，把SR[sr1StartIndex..sr1EndIndex]和SR[sr1EndIndex+1..sr2EndIndex]归并为有序的TR[sr1StartIndex..sr2EndIndex]
        // /// </summary>
        // /// <param name="sr"></param>
        // /// <param name="tr"></param>
        // /// <param name="sr1StartIndex"></param>
        // /// <param name="sr1EndIndex"></param>
        // /// <param name="sr2EndIndex"></param>
        // private static void Merge(int[] sr, int[] tr, int sr1StartIndex, int sr1EndIndex, int sr2EndIndex)
        // {
        //     int sr2StartIndex, currentProcess; //currentProcess为当前进度
        //
        //     for (sr2StartIndex = sr1EndIndex + 1, currentProcess = sr1StartIndex;
        //         sr1StartIndex <= sr1EndIndex && sr2StartIndex <= sr2EndIndex;
        //         currentProcess++) //两个SR有一个被榨干后就要退出循环
        //     {
        //         if (sr[sr1StartIndex] < sr[sr2StartIndex])
        //         {
        //             tr[currentProcess] = sr[sr1StartIndex++];
        //         }
        //         else
        //         {
        //             tr[currentProcess] = sr[sr2StartIndex++];
        //         }
        //     }
        //
        //     if (sr1StartIndex <= sr1EndIndex)
        //     {
        //         for (int l = 0; l <= sr1EndIndex - sr1StartIndex; l++)
        //         {
        //             tr[currentProcess + l] = sr[sr1StartIndex + l]; //将剩余的SR[sr1StartIndex...sr1EndIndex]复制到TR
        //         }
        //     }
        //
        //     if (sr2StartIndex <= sr2EndIndex)
        //     {
        //         for (int l = 0; l <= sr2EndIndex - sr2StartIndex; l++)
        //         {
        //             tr[currentProcess + l] = sr[sr2StartIndex + l]; //将剩余的SR[sr2StartIndex...sr2EndIndex]复制到TR
        //         }
        //     }
        // }
        //
        // private void QuickSort(int[] array, int count)
        // {
        //     QSort(array, 0, count - 1);
        // }
        //
        // private static void QSort(int[] array, int low, int high)
        // {
        //     int pivot;
        //
        //     while (low < high)
        //     {
        //         pivot = Partition(array, low, high);
        //         QSort(array, low, pivot - 1);
        //         //尾递归，可以减少一次递归堆栈深度
        //         low = pivot + 1;
        //     }
        // }
        //
        // private static int Partition(int[] array, int low, int high)
        // {
        //     int pivotkey;
        //     int m = low + (high - low) / 2;
        //
        //     //下面是三数取中优化
        //     //交换左端与右端数据，保证左端较小
        //     if (array[low] > array[high])
        //     {
        //         XorSwap(ref array[low], ref array[high]);
        //     }
        //
        //     //交换中间与右端数据，保证中间较小
        //     if (array[m] > array[high])
        //     {
        //         XorSwap(ref array[m], ref array[high]);
        //     }
        //
        //     //交换中间与左端数据，保证左端较小
        //     if (array[m] > array[low])
        //     {
        //         XorSwap(ref array[low], ref array[m]);
        //     }
        //
        //     //默认选取当前数组的第一个值作为枢轴值
        //     pivotkey = array[low];
        //     //枢轴备份
        //     int pivotkeyback = pivotkey;
        //
        //     while (low < high)
        //     {
        //         while (low < high && array[high] >= pivotkey)
        //         {
        //             high--;
        //         }
        //
        //         array[low] = array[high];
        //
        //         while (low < high && array[low] <= pivotkey)
        //         {
        //             low++;
        //         }
        //
        //         array[high] = array[low];
        //     }
        //
        //     //将枢轴数值替换回array[low]
        //     array[low] = pivotkeyback;
        //
        //     //返回当前枢轴下标
        //     return low;
        // }

        private static void XorSwap(ref int a, ref int b)
        {
            a ^= b;
            b ^= a;
            a ^= b;
        }
    }
}