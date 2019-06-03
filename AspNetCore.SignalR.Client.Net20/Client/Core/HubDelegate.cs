namespace AspNetCore.SignalR.Client.Net20.Client
{
    public delegate void Action();
    public delegate void Action<T1>(T1 t1);
    public delegate void Action<T1, T2>(T1 t1, T2 t2);
    public delegate void Action<T1, T2, T3>(T1 t1, T2 t2, T3 t3);
    public delegate void Action<T1, T2, T3, T4>(T1 t1, T2 t2, T3 t3, T4 t4);
    public delegate void Action<T1, T2, T3, T4, T5>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5);
    public delegate void Action<T1, T2, T3, T4, T5, T6>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6);
    public delegate void Action<T1, T2, T3, T4, T5, T6, T7>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7);
    public delegate void Action<T1, T2, T3, T4, T5, T6, T7, T8>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8);
    public delegate void Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9);
    public delegate void Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10);

    public delegate TResult Func<out TResult>();
    public delegate TResult Func<T1, out TResult>(T1 t1);
    public delegate TResult Func<T1, T2, out TResult>(T1 t1, T2 t2);
    public delegate TResult Func<T1, T2, T3, out TResult>(T1 t1, T2 t2, T3 t3);
    public delegate TResult Func<T1, T2, T3, T4, out TResult>(T1 t1, T2 t2, T3 t3, T4 t4);
    public delegate TResult Func<T1, T2, T3, T4, T5, out TResult>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5);
    public delegate TResult Func<T1, T2, T3, T4, T5, T6, out TResult>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6);
    public delegate TResult Func<T1, T2, T3, T4, T5, T6, T7, out TResult>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7);
    public delegate TResult Func<T1, T2, T3, T4, T5, T6, T7, T8, out TResult>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8);
    public delegate TResult Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, out TResult>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9);
    public delegate TResult Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, out TResult>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10);
}
