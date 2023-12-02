namespace Console_TestTask.Context.Models
{
    /// <summary>
    /// класс работника
    /// </summary>
    public class Employee
    {  

        /// <summary>
        /// автосвойство позволяющее получать и задавать идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// автосвойство позволяющее получать и задавать ФИО
        /// </summary>
        public string FIO { get; set; }

        /// <summary>
        /// автосвойство позволяющее получать и задавать дату рождения
        /// </summary>
        public DateOnly DateOfBirth { get; set; }

        /// <summary>
        /// автосвойство позволяющее получать и задавать пол
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// пустой конструктор (для создания объекта БД с помощью Entity Framework Code First)
        /// </summary>
        public Employee()
        {            
        }

        /// <summary>
        /// конструктор с параметрами
        /// </summary>
        /// <param name="fio"></param>
        /// <param name="dateOfBirth"></param>
        /// <param name="gender"></param>
        public Employee(string fio, DateOnly dateOfBirth, Gender gender) : base ()
        {
            FIO = fio;
            DateOfBirth = dateOfBirth;
            Gender = gender.ToString();
        }

        /// <summary>
        /// метод возвращающий возраст работника
        /// </summary>
        /// <returns>возраст сотрудника</returns>
        public int GetAge()
        {
            int age = DateTime.Now.Year - DateOfBirth.Year;
            age = DateTime.Now.DayOfYear <= DateOfBirth.DayOfYear ? ++age : age;
            return age;
        }

        /// <summary>
        /// переопределение метода ToString()
        /// </summary>
        /// <returns>форматированная строка с данными сотрудника</returns>
        public override string ToString()
        {
            return $"ФИО: {FIO} Возраст: {GetAge()} Пол: {Gender}";
        }
    }

}
