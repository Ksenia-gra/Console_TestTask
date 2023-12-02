namespace Console_TestTask.DataGenerators
{
    /// <summary>
    /// интерфейс генератора
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGenerator<T>
    {
        /// <summary>
        /// обобщенный метод для генерации объекта
        /// </summary>
        T Generate();
    }
}
