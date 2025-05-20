namespace WorkTimeTable.DataBase
{
    // Здесь определяется структуру БД

    /// <summary>
    /// Определяет таблицу БД Worker (Работник).
    /// </summary>
    public class Worker
    {
        public int Id { get; set; } // первичный ключ
        public string Name { get; set; } = "Н/Д";
        public string Position { get; set; } = "Н/Д";

        // связь между таблицами
        public Department? Department { get; set; } // навигационное свойство
        public int? DepartmentId { get; set; } // внешний ключ

        public Department? DepartmentUnderControl { get; set; } // зависимая сущность
        public List<Timetable> Timetables { get; set; } = []; // зависимая сущность; у работника может быть много договоров

        public Worker() { }

        public Worker(string name, string position, Department? department = null)
        {
            Name = name;
            Position = position;
            Department = department;
        }
    }

    /// <summary>
    /// Определяет таблицу БД  Department (Отдел).
    /// </summary>
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Н/Д";

        // связь между таблицами
        public Worker? Leader { get; set; } // навигационное свойство
        public int? LeaderId { get; set; } // внешний ключ

        public List<Worker> Workers { get; set; } = []; // зависимая сущность; типа у отдела может быть много работников

        public Department() { }

        public Department(string name)
        {
            Name = name;
        }

    }

    /// <summary>
    /// Определяет таблицу БД Contract (договор).
    /// </summary>
    public class Contract
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Н/Д";

        // связь между таблицами
        public List<Timetable> Timetables { get; set; } = []; // зависимая сущность, у договора может быть много работников

        public Contract() { }

        public Contract(string name)
        {
            Name = name;
        }
    }

    /// <summary>
    /// Определяет таблицу БД Timetable (Рабочий табель). Выходные данные. Связь "многие-ко-многим" между таблицами Worker и Contract
    /// </summary>
    public class Timetable
    {
        public int Id { get; set; }

        // связь между таблицами
        public Contract Contract { get; set; } = null!; // навигационное свойство
        public int ContractId { get; set; } // внешний ключ
        public Worker Worker { get; set; } = null!; // навигационное свойство
        public int Workerid { get; set; } // внешний ключ

        public Timetable() { }

    }

    /// <summary>
    /// Определяет иерархию между отделами в БД
    /// </summary>
    public class DepartmentHierarchy
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }
        public DepartmentHierarchy? Parent { get; set; }
        public List<DepartmentHierarchy> Children { get; set; } = [];


    }
}
