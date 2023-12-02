using Console_TestTask.Context.Models;

namespace Console_TestTask.DataGenerators
{
    /// <summary>
    /// интерфейс генератора работника
    /// </summary>
    public interface IEmployeeGenerator : IGenerator<Employee>
    {
        /// <summary>
        /// метод для генерации даты рождения
        /// </summary>
        DateOnly GenerateDateOfBirth();

        /// <summary>
        /// метод для генерации ФИО
        /// </summary>
        string GenerateFIO();

        /// <summary>
        /// перегрузка метода для генерации ФИО с параметром
        /// </summary>
        /// <param name="fioPattern"></param>
        string GenerateFIO(string fioPattern);

        /// <summary>
        /// метод для генерации пола
        /// </summary>
        Gender GenerateGender();
    }
}