const char DEL = '-';
const int DEL_LENGTH = 30;
Dictionary<ConsoleKey, Type> TYPE_BY_KEY = new()
{
    { ConsoleKey.D1, typeof(uint) },
    { ConsoleKey.D2, typeof(int) },
    { ConsoleKey.D3, typeof(long) },
    { ConsoleKey.D4, typeof(float) },
    { ConsoleKey.D5, typeof(double) },
    { ConsoleKey.D6, typeof(char) },
    { ConsoleKey.D7, typeof(string) },
    // TODO: после переезда на 10 версию дотнета расскомментить 
    //{ ConsoleKey.D8, typeof(record) },
    { ConsoleKey.D9, typeof(Tuple<int, string>) },
};

void ChooseTypeFromList()
{

    while (true)
    {
        Console.WriteLine(
            "Информация по типам\n" +
            new string(
                DEL,
                DEL_LENGTH
            ) + '\n' +
            "\t1 - uint\n" +
            "\t2 - int\n" +
            "\t3 - long\n" +
            "\t4 - float\n" +
            "\t5 - double\n" +
            "\t6 - char\n" +
            "\t7 - string\n" +
            "\t8 - record\n" +
            "\t9 - Type<int, string>\n" +
            "\t0 - Выход в главное меню\n"
        );

        ConsoleKey key = Console.ReadKey().Key;

        if (key == ConsoleKey.D0)
        {
            return;
        }

        var type = TYPE_BY_KEY.GetValueOrDefault(key);

        if (type == null)
        {
            continue;
        }
    }
}

while (true)
{
    Console.WriteLine(
        "Информация по типам:\n" +
        "1 - Общая информация по типам\n" +
        "2 - Выбрать тип из списка\n" +
        "3 - Параметры консоли\n" +
        "0 - Выход из программы"
        );

    ConsoleKey key = Console.ReadKey().Key;

    switch (key)
    {
        case ConsoleKey.D2:
            ChooseTypeFromList();
            break;
        case ConsoleKey.D0:
            return;
        default:
            break;

    }
}