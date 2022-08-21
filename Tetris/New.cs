
int mapWidth = 10; // Переменная содержащая ширину игравого пространства
int allMapWidth = 15; // Переменная содержащая общюю ширину окна
int mapHeight = 20; // Переменная содержащая высоту окна
int interval = 500; // Переменная содержащая определенный интервал времени

//Переменные ниже используются для увеличения окна игры и взаимодействия с ним
int screenWidth = allMapWidth * 3;
int screenHeight = mapHeight * 3;
int gameScreenWidth = mapWidth * 3;

ConsoleColor stuffColor = ConsoleColor.Green; // Переменная содержащая цвет символов
int[,] map = new int[mapWidth, mapHeight]; // Массив отражает текущее состояние игрового поля (0 ячейкас свободна. 1-4 ячейки заняты элементом фгуры с соответствующим номером. 5 ячейка занята элементом старых фигур)
int[,] temp = new int[mapWidth, mapHeight]; // Массив в которохом происходят изменения до проверки их возможности

int startX = 3; // переменная содержащая начальное положение первого элемента фигуры по оси Х (строки)
int startY = 0; // переменная содержащая начальное положение первого элемента фигуры по оси У (столбцы)
int score = 0; // переменная содержащая начальное количество очков
int type; // переменная в которую будет записан номер типа фигуры
int versionElement; // номер версии элемента

int[] firstPixelPosition = new int[2]; // массив хранящий индексы 1 пикселя в массиве map
int[] secondPixelPosition = new int[2]; // массив хранящий индексы 2 пикселя в массиве map
int[] thirdPixelPosition = new int[2]; // массив хранящий индексы 3 пикселя в массиве map
int[] fourthPixelPosition = new int[2]; // массив хранящий индексы 4 пикселя в массиве map

// ниже задается размер окна
Console.SetWindowSize(screenWidth, screenHeight);
Console.SetBufferSize(screenWidth, screenHeight);

Console.CursorVisible = false; // свойство определяющее видимость курсора 

// Ниже код реализующий выбор уровня сложности
int position = 25;
Console.SetCursorPosition(15, 25);
Console.Write($"EASY");
Console.SetCursorPosition(15, 27);
Console.Write($"MEDIUM");
Console.SetCursorPosition(15, 29);
Console.Write($"HARD");
Console.SetCursorPosition(12, position);
Console.Write($"=>");

while (true)
{
    ConsoleKey enter = Console.ReadKey(true).Key;
    Thread.Sleep(100);
    if (enter == ConsoleKey.DownArrow)
    {
        Console.SetCursorPosition(12, position);
        Console.Write($"  ");
        position += 2;
        if (position == 31) position = 25;
        Console.SetCursorPosition(12, position);
        Console.Write($"=>");
    }
    if (enter == ConsoleKey.UpArrow)
    {
        Console.SetCursorPosition(12, position);
        Console.Write($"  ");
        position -= 2;
        if (position == 23) position = 29;
        Console.SetCursorPosition(12, position);
        Console.Write($"=>");
    }
    if (enter == ConsoleKey.Enter) break;
    while (Console.KeyAvailable == true) // без этого цикла при многократном нажатии клавиш управления, значения нажатой клавиши передовалось в следующее движение столько раз сколько была нажата клавиша
    {
        ConsoleKey t = Console.ReadKey(true).Key;
    }
}
if (position == 25) interval = 700;
if (position == 27) interval = 550;
if (position == 29) interval = 400;
Console.Clear(); // очистка всего экрана




while (true) // на данный момент игра продолжается бесконечно, условий для завершения игры нет
{
    // ниже прописано увеличение сложности в зависимости от количества очков
    int levelGame = 0;
    if (score == 40 && levelGame == 0)
    {
        interval -= 30;
        levelGame = 1;
    }
    if (score == 80 && levelGame == 1)
    {
        interval -= 30;
        levelGame = 2;
    }
    if (score == 120 && levelGame == 2)
    {
        interval -= 30;
        levelGame = 3;
    }
    if (score == 140 && levelGame == 3)
    {
        interval -= 30;
        levelGame = 4;
    }

    versionElement = 0; // Характеризует базовое состояние элемента
    type = new Random().Next(0, 7); // запись в переменную случайного числа

    // дальше в зависимости от значения переменной type выбирается одна из фигур
    if (type == 0) Line();
    if (type == 1) Box();
    if (type == 2) BlockLLeft();
    if (type == 3) BlockLRight();
    if (type == 4) BlockT();
    if (type == 5) BlockZLeft();
    if (type == 6) BlockZRight();
    Draw(); // Метод отрисовывающий текущее состояние экрана
    Thread.Sleep(interval);
    while (true)
    {
        if (Console.KeyAvailable == false) // если нет нажатия клавиши то при выполнении условий происходит двежение вниз
        {
            // условие проверяет не заняты ли нижние участки игрового поля и не закончилось ли поле
            if (firstPixelPosition[1] + 1 < map.GetLength(1)
                && secondPixelPosition[1] + 1 < map.GetLength(1)
                && thirdPixelPosition[1] + 1 < map.GetLength(1)
                && fourthPixelPosition[1] + 1 < map.GetLength(1)
                && map[firstPixelPosition[0], firstPixelPosition[1] + 1] != 5
                && map[secondPixelPosition[0], secondPixelPosition[1] + 1] != 5
                && map[thirdPixelPosition[0], thirdPixelPosition[1] + 1] != 5
                && map[fourthPixelPosition[0], fourthPixelPosition[1] + 1] != 5)
            {
                MoveDown(); // метод выполняет движение вниз
            }
            else // если вниз двигаться нельзя то элементам массива с текущими координатами фигуры присваивается значение 5.
            {
                map[firstPixelPosition[0], firstPixelPosition[1]] = 5;
                map[secondPixelPosition[0], secondPixelPosition[1]] = 5;
                map[thirdPixelPosition[0], thirdPixelPosition[1]] = 5;
                map[fourthPixelPosition[0], fourthPixelPosition[1]] = 5;

                break; // выход из цикла
            }
        }
        else // дальше при условии нажатии одной из задействованных клавиш производится соответсвующее действие
        {
            ConsoleKey key = Console.ReadKey(true).Key;//Переменной key присваивается значение нажатой клавиши 

            /*Дальше проверяется условие на возможность движения влевоб если правда то вызывается метод который перемещает элемент влево
             если нет то проверяется условие на возможность движения вниз и если вниз можно то происходит движение вниз
            после чего обновляется массив map и продолжается выполнение цикла. Если двигаться вниз нельзя то элементам массива с текущими координатами фигуры присваивается значение 5.
             */
            if (key == ConsoleKey.LeftArrow)
            {
                if (firstPixelPosition[0] - 1 >= 0
                    && secondPixelPosition[0] - 1 >= 0
                    && thirdPixelPosition[0] - 1 >= 0
                    && fourthPixelPosition[0] - 1 >= 0
                    && map[firstPixelPosition[0] - 1, firstPixelPosition[1]] != 5
                    && map[secondPixelPosition[0] - 1, secondPixelPosition[1]] != 5
                    && map[thirdPixelPosition[0] - 1, thirdPixelPosition[1]] != 5
                    && map[fourthPixelPosition[0] - 1, fourthPixelPosition[1]] != 5)
                {
                    MoveLeft();
                }
                if (firstPixelPosition[1] + 1 < map.GetLength(1)
                    && secondPixelPosition[1] + 1 < map.GetLength(1)
                    && thirdPixelPosition[1] + 1 < map.GetLength(1)
                    && fourthPixelPosition[1] + 1 < map.GetLength(1)
                    && map[firstPixelPosition[0], firstPixelPosition[1] + 1] != 5
                    && map[secondPixelPosition[0], secondPixelPosition[1] + 1] != 5
                    && map[thirdPixelPosition[0], thirdPixelPosition[1] + 1] != 5
                    && map[fourthPixelPosition[0], fourthPixelPosition[1] + 1] != 5)
                {
                    MoveDown();
                }
                else
                {
                    map[firstPixelPosition[0], firstPixelPosition[1]] = 5;
                    map[secondPixelPosition[0], secondPixelPosition[1]] = 5;
                    map[thirdPixelPosition[0], thirdPixelPosition[1]] = 5;
                    map[fourthPixelPosition[0], fourthPixelPosition[1]] = 5;
                    break;
                }

            }
            else if (key == ConsoleKey.RightArrow) // см. коментарий к предыдущему условию (только в данном случае движение вправо)
            {
                if (firstPixelPosition[0] + 1 < map.GetLength(0)
                && secondPixelPosition[0] + 1 < map.GetLength(0)
                && thirdPixelPosition[0] + 1 < map.GetLength(0)
                && fourthPixelPosition[0] + 1 < map.GetLength(0)
                && map[firstPixelPosition[0] + 1, firstPixelPosition[1]] != 5
                && map[secondPixelPosition[0] + 1, secondPixelPosition[1]] != 5
                && map[thirdPixelPosition[0] + 1, thirdPixelPosition[1]] != 5
                && map[fourthPixelPosition[0] + 1, fourthPixelPosition[1]] != 5)
                {
                    MoveRight();
                }
                if (firstPixelPosition[1] + 1 < map.GetLength(1)
                   && secondPixelPosition[1] + 1 < map.GetLength(1)
                   && thirdPixelPosition[1] + 1 < map.GetLength(1)
                   && fourthPixelPosition[1] + 1 < map.GetLength(1)
                   && map[firstPixelPosition[0], firstPixelPosition[1] + 1] != 5
                   && map[secondPixelPosition[0], secondPixelPosition[1] + 1] != 5
                   && map[thirdPixelPosition[0], thirdPixelPosition[1] + 1] != 5
                   && map[fourthPixelPosition[0], fourthPixelPosition[1] + 1] != 5)
                {
                    MoveDown();
                }
                else
                {
                    map[firstPixelPosition[0], firstPixelPosition[1]] = 5;
                    map[secondPixelPosition[0], secondPixelPosition[1]] = 5;
                    map[thirdPixelPosition[0], thirdPixelPosition[1]] = 5;
                    map[fourthPixelPosition[0], fourthPixelPosition[1]] = 5;

                    break;
                }

            }
            else if (key == ConsoleKey.DownArrow) // см. коментарий к предыдущему условию (только в данном случае движение вправо)
            {
                while (true)
                {
                    if (firstPixelPosition[1] + 1 < map.GetLength(1)
                       && secondPixelPosition[1] + 1 < map.GetLength(1)
                       && thirdPixelPosition[1] + 1 < map.GetLength(1)
                       && fourthPixelPosition[1] + 1 < map.GetLength(1)
                       && map[firstPixelPosition[0], firstPixelPosition[1] + 1] != 5
                       && map[secondPixelPosition[0], secondPixelPosition[1] + 1] != 5
                       && map[thirdPixelPosition[0], thirdPixelPosition[1] + 1] != 5
                       && map[fourthPixelPosition[0], fourthPixelPosition[1] + 1] != 5)
                    {
                        MoveDown();
                    }
                    else
                    {
                        map[firstPixelPosition[0], firstPixelPosition[1]] = 5;
                        map[secondPixelPosition[0], secondPixelPosition[1]] = 5;
                        map[thirdPixelPosition[0], thirdPixelPosition[1]] = 5;
                        map[fourthPixelPosition[0], fourthPixelPosition[1]] = 5;

                        break;
                    }
                }
                break;

            }
            else if (key == ConsoleKey.Spacebar)
            {
                Console.SetCursorPosition(15, 25);
                Console.Write($"Pause");
                while (Console.KeyAvailable == false)
                {

                    Thread.Sleep(500);
                }
                Console.SetCursorPosition(15, 25);
                Console.Write($"     ");


            }
            /* стрелка вверх запускает метод который поворачивает фигуру. После поворота происходт проверка не попала ли фигура куда ей
              нельзя и если попала она еще раз поворачивается и тем самым возвращается в исходное положение.
              Дальше аналогично предыдущим комментариям происходит движение вниз.*/
            else if (key == ConsoleKey.UpArrow)
            {
                Array.Copy(map, map.GetLowerBound(0), temp, temp.GetLowerBound(0), map.GetLength(0) * map.GetLength(1));
                MoveChange();

                if (map[firstPixelPosition[0], firstPixelPosition[1]] == 5
                    || map[secondPixelPosition[0], secondPixelPosition[1]] == 5
                    || map[thirdPixelPosition[0], thirdPixelPosition[1]] == 5
                    || map[fourthPixelPosition[0], fourthPixelPosition[1]] == 5)
                {
                    if (type == 2 || type == 3) for (int i = 0; i < 2; i++) MoveChange();
                    MoveChange();
                }
                map = FillMap(map, temp);

                if (firstPixelPosition[1] + 1 < map.GetLength(1)
                   && secondPixelPosition[1] + 1 < map.GetLength(1)
                   && thirdPixelPosition[1] + 1 < map.GetLength(1)
                   && fourthPixelPosition[1] + 1 < map.GetLength(1)
                   && map[firstPixelPosition[0], firstPixelPosition[1] + 1] != 5
                   && map[secondPixelPosition[0], secondPixelPosition[1] + 1] != 5
                   && map[thirdPixelPosition[0], thirdPixelPosition[1] + 1] != 5
                   && map[fourthPixelPosition[0], fourthPixelPosition[1] + 1] != 5)
                {
                    MoveDown();
                }
                else
                {
                    map[firstPixelPosition[0], firstPixelPosition[1]] = 5;
                    map[secondPixelPosition[0], secondPixelPosition[1]] = 5;
                    map[thirdPixelPosition[0], thirdPixelPosition[1]] = 5;
                    map[fourthPixelPosition[0], fourthPixelPosition[1]] = 5;

                    break;
                }

            }
            else // это условие добавлено на случай нажатия любой другой клавиши и аналогично бездействию и дает движение вниз
            {
                if (firstPixelPosition[1] + 1 < map.GetLength(1)
                    && secondPixelPosition[1] + 1 < map.GetLength(1)
                    && thirdPixelPosition[1] + 1 < map.GetLength(1)
                    && fourthPixelPosition[1] + 1 < map.GetLength(1)
                    && map[firstPixelPosition[0], firstPixelPosition[1] + 1] != 5
                    && map[secondPixelPosition[0], secondPixelPosition[1] + 1] != 5
                    && map[thirdPixelPosition[0], thirdPixelPosition[1] + 1] != 5
                    && map[fourthPixelPosition[0], fourthPixelPosition[1] + 1] != 5)
                {
                    MoveDown();
                }
                else
                {
                    map[firstPixelPosition[0], firstPixelPosition[1]] = 5;
                    map[secondPixelPosition[0], secondPixelPosition[1]] = 5;
                    map[thirdPixelPosition[0], thirdPixelPosition[1]] = 5;
                    map[fourthPixelPosition[0], fourthPixelPosition[1]] = 5;

                    break;
                }
            }
        }
        while (Console.KeyAvailable == true) // без этого цикла при многократном нажатии клавиш управления, значения нажатой клавиши передовалось в следующее движение столько раз сколько была нажата клавиша
        {
            ConsoleKey t = Console.ReadKey(true).Key;
        }
        Console.Clear(); // очистка всего экрана
        Draw(); // Отрисовка текущего состояния экрана
        Thread.Sleep(interval); // приостановка программы на время записанное в переменной interval в милисекундах
    }
    Console.Clear(); // очистка всего экрана
    score = CountScore(map, score); //метод считает очки
    map = ClearMap(map); //метод удаляет заполненные строки
    Draw(); // Отрисовка текущего состояния экрана
    Thread.Sleep(interval); // приостановка программы на время записанное в переменной interval в милисекундах
}




// метод принимает массив и цвет значков и отрисовывает игровое поле.
void Draw()
{
    char pixelChar = '█';
    char pixelCharOne = '@';
    Console.ForegroundColor = stuffColor;
    for (int i = 0; i < map.GetLength(0); i++)
    {
        for (int j = 0; j < map.GetLength(1); j++)
        {
            if (map[i, j] == 5)
            {
                for (int k = 0; k < 3; k++)
                {
                    for (int m = 0; m < 3; m++)
                    {
                        Console.SetCursorPosition(i * 3 + k, j * 3 + m);
                        Console.Write(pixelChar);
                    }
                }

            }
            else if (map[i, j] != 0)
            {
                for (int k = 0; k < 3; k++)
                {
                    for (int m = 0; m < 3; m++)
                    {
                        Console.SetCursorPosition(i * 3 + k, j * 3 + m);
                        Console.Write(pixelCharOne);
                    }
                }
            }

        }
    }
    for (int i = 0; i < screenHeight; i++)
    {
        Console.SetCursorPosition(gameScreenWidth, i);
        Console.Write("|");
    }
    Console.SetCursorPosition(32, 20);
    Console.Write($"Score:{score}");
    Console.SetCursorPosition(32, 25);
    Console.Write($"Type:{type}");

}

// Метод берет два массива, если значение перемменой в массиве не равно 5 то он присваевает этой переменной значение равное значению переменной с темеже индесами из второго массива
int[,] FillMap(int[,] firstArray, int[,] secondArray)
{
    for (int i = 0; i < firstArray.GetLength(0); i++)
    {
        for (int j = 0; j < firstArray.GetLength(1); j++)
        {
            if (firstArray[i, j] != 5) firstArray[i, j] = secondArray[i, j];
        }
    }
    return firstArray;
}
// метод принимает массив и текущее количество очков, при условии что какая либо строка полностью занята количество очков увеличивается на 10 за каждую строку. Метод возвращает новое значение количества очков.
int CountScore(int[,] array, int score)
{
    for (int i = 0; i < array.GetLength(1); i++)
    {
        int sum = 0;
        for (int j = 0; j < array.GetLength(0); j++)
        {
            sum = sum + map[j, i];
        }
        if (sum == 50) score = score + 10;
    }
    return score;
}

/* Метод принимает массив. Если сумма в какой либо строке равна 50 (тоесть вся строка занята)
 то последовательно для этой и всех верхних строк элементам строки присваивается значение элементов строки сверху
 */
int[,] ClearMap(int[,] array)
{
    for (int i = 0; i < array.GetLength(1); i++)
    {
        int sum = 0;
        for (int j = 0; j < array.GetLength(0); j++)
        {
            sum = sum + array[j, i];
        }
        if (sum == 50)
        {
            for (int k = i; k > 0; k--)
            {
                for (int m = 0; m < array.GetLength(0); m++)
                {
                    array[m, k] = array[m, k - 1];
                }
            }
        }
    }
    return array;
}

void Line()
{
    map[startX, startY] = 1;
    map[startX + 1, startY] = 2;
    map[startX + 2, startY] = 3;
    map[startX + 3, startY] = 4;

    firstPixelPosition[0] = startX;
    firstPixelPosition[1] = startY;
    secondPixelPosition[0] = startX + 1;
    secondPixelPosition[1] = startY;
    thirdPixelPosition[0] = startX + 2;
    thirdPixelPosition[1] = startY;
    fourthPixelPosition[0] = startX + 3;
    fourthPixelPosition[1] = startY;
}
void Box()
{
    map[startX, startY] = 1;
    map[startX + 1, startY] = 2;
    map[startX, startY + 1] = 3;
    map[startX + 1, startY + 1] = 4;

    firstPixelPosition[0] = startX;
    firstPixelPosition[1] = startY;
    secondPixelPosition[0] = startX + 1;
    secondPixelPosition[1] = startY;
    thirdPixelPosition[0] = startX;
    thirdPixelPosition[1] = startY + 1;
    fourthPixelPosition[0] = startX + 1;
    fourthPixelPosition[1] = startY + 1;
}
void BlockLLeft()
{
    map[startX, startY + 1] = 1;
    map[startX, startY] = 2;
    map[startX + 1, startY] = 3;
    map[startX + 2, startY] = 4;

    firstPixelPosition[0] = startX;
    firstPixelPosition[1] = startY + 1;
    secondPixelPosition[0] = startX;
    secondPixelPosition[1] = startY;
    thirdPixelPosition[0] = startX + 1;
    thirdPixelPosition[1] = startY;
    fourthPixelPosition[0] = startX + 2;
    fourthPixelPosition[1] = startY;
}
void BlockLRight()
{
    map[startX, startY] = 1;
    map[startX + 1, startY] = 2;
    map[startX + 2, startY] = 3;
    map[startX + 2, startY + 1] = 4;

    firstPixelPosition[0] = startX;
    firstPixelPosition[1] = startY;
    secondPixelPosition[0] = startX + 1;
    secondPixelPosition[1] = startY;
    thirdPixelPosition[0] = startX + 2;
    thirdPixelPosition[1] = startY;
    fourthPixelPosition[0] = startX + 2;
    fourthPixelPosition[1] = startY + 1;
}
void BlockT()
{
    map[startX, startY] = 1;
    map[startX + 1, startY] = 2;
    map[startX + 2, startY] = 3;
    map[startX + 1, startY + 1] = 4;

    firstPixelPosition[0] = startX;
    firstPixelPosition[1] = startY;
    secondPixelPosition[0] = startX + 1;
    secondPixelPosition[1] = startY;
    thirdPixelPosition[0] = startX + 2;
    thirdPixelPosition[1] = startY;
    fourthPixelPosition[0] = startX + 1;
    fourthPixelPosition[1] = startY + 1;
}
void BlockZLeft()
{
    map[startX, startY] = 1;
    map[startX + 1, startY] = 2;
    map[startX + 1, startY + 1] = 3;
    map[startX + 2, startY + 1] = 4;

    firstPixelPosition[0] = startX;
    firstPixelPosition[1] = startY;
    secondPixelPosition[0] = startX + 1;
    secondPixelPosition[1] = startY;
    thirdPixelPosition[0] = startX + 1;
    thirdPixelPosition[1] = startY + 1;
    fourthPixelPosition[0] = startX + 2;
    fourthPixelPosition[1] = startY + 1;
}
void BlockZRight()
{
    map[startX, startY + 1] = 1;
    map[startX + 1, startY + 1] = 2;
    map[startX + 1, startY] = 3;
    map[startX + 2, startY] = 4;

    firstPixelPosition[0] = startX;
    firstPixelPosition[1] = startY + 1;
    secondPixelPosition[0] = startX + 1;
    secondPixelPosition[1] = startY + 1;
    thirdPixelPosition[0] = startX + 1;
    thirdPixelPosition[1] = startY;
    fourthPixelPosition[0] = startX + 2;
    fourthPixelPosition[1] = startY;
}
void MoveDown() //метод реализующий движение вниз.
{
    // элементам массива которые заняты на данный момент присваивается значение 0
    ClearOldPixel(); ;
    map[firstPixelPosition[0], firstPixelPosition[1]] = 0;
    map[secondPixelPosition[0], secondPixelPosition[1]] = 0;
    map[thirdPixelPosition[0], thirdPixelPosition[1]] = 0;
    map[fourthPixelPosition[0], fourthPixelPosition[1]] = 0;
    // элементам массива которые занимаются при движении присваивается не нулевое значение
    map[firstPixelPosition[0], firstPixelPosition[1] + 1] = 1;
    map[secondPixelPosition[0], secondPixelPosition[1] + 1] = 2;
    map[thirdPixelPosition[0], thirdPixelPosition[1] + 1] = 3;
    map[fourthPixelPosition[0], fourthPixelPosition[1] + 1] = 4;
    //В массивах хранящих адрес каждого элемента в массиве currentPosition актуализируются значения
    firstPixelPosition[1] += 1;
    secondPixelPosition[1] += 1;
    thirdPixelPosition[1] += 1;
    fourthPixelPosition[1] += 1;
}
void MoveLeft() // см. комментарии к движению вниз
{

    ClearOldPixel();
    map[firstPixelPosition[0], firstPixelPosition[1]] = 0;
    map[secondPixelPosition[0], secondPixelPosition[1]] = 0;
    map[thirdPixelPosition[0], thirdPixelPosition[1]] = 0;
    map[fourthPixelPosition[0], fourthPixelPosition[1]] = 0;

    map[firstPixelPosition[0] - 1, firstPixelPosition[1]] = 1;
    map[secondPixelPosition[0] - 1, secondPixelPosition[1]] = 2;
    map[thirdPixelPosition[0] - 1, thirdPixelPosition[1]] = 3;
    map[fourthPixelPosition[0] - 1, fourthPixelPosition[1]] = 4;

    firstPixelPosition[0] -= 1;
    secondPixelPosition[0] -= 1;
    thirdPixelPosition[0] -= 1;
    fourthPixelPosition[0] -= 1;
}
void MoveRight() // см. комментарии к движению вниз
{
    ClearOldPixel();
    map[firstPixelPosition[0], firstPixelPosition[1]] = 0;
    map[secondPixelPosition[0], secondPixelPosition[1]] = 0;
    map[thirdPixelPosition[0], thirdPixelPosition[1]] = 0;
    map[fourthPixelPosition[0], fourthPixelPosition[1]] = 0;

    map[firstPixelPosition[0] + 1, firstPixelPosition[1]] = 1;
    map[secondPixelPosition[0] + 1, secondPixelPosition[1]] = 2;
    map[thirdPixelPosition[0] + 1, thirdPixelPosition[1]] = 3;
    map[fourthPixelPosition[0] + 1, fourthPixelPosition[1]] = 4;

    firstPixelPosition[0] += 1;
    secondPixelPosition[0] += 1;
    thirdPixelPosition[0] += 1;
    fourthPixelPosition[0] += 1;
}
void MoveChange() // метод реализующий поворот фигуры. вначале проверяет возможен ли поворот а потом все аналогично комментариям к движению вниз
{
    if (type == 0)
    {
        if (versionElement == 0)
        {
            if (firstPixelPosition[1] + 3 < temp.GetLength(1))
            {
                temp[firstPixelPosition[0], firstPixelPosition[1]] = 0;
                temp[secondPixelPosition[0], secondPixelPosition[1]] = 0;
                temp[thirdPixelPosition[0], thirdPixelPosition[1]] = 0;
                temp[fourthPixelPosition[0], fourthPixelPosition[1]] = 0;

                temp[firstPixelPosition[0], firstPixelPosition[1] + 3] = 1;
                temp[secondPixelPosition[0] - 1, secondPixelPosition[1] + 2] = 2;
                temp[thirdPixelPosition[0] - 2, thirdPixelPosition[1] + 1] = 3;
                temp[fourthPixelPosition[0] - 3, fourthPixelPosition[1]] = 4;

                firstPixelPosition = Check(1, temp);
                secondPixelPosition = Check(2, temp);
                thirdPixelPosition = Check(3, temp);
                fourthPixelPosition = Check(4, temp);
                versionElement = 1;
            }
        }
        else
        {
            if (fourthPixelPosition[0] + 3 < temp.GetLength(0))
            {
                temp[firstPixelPosition[0], firstPixelPosition[1]] = 0;
                temp[secondPixelPosition[0], secondPixelPosition[1]] = 0;
                temp[thirdPixelPosition[0], thirdPixelPosition[1]] = 0;
                temp[fourthPixelPosition[0], fourthPixelPosition[1]] = 0;

                temp[firstPixelPosition[0], firstPixelPosition[1] - 3] = 1;
                temp[secondPixelPosition[0] + 1, secondPixelPosition[1] - 2] = 2;
                temp[thirdPixelPosition[0] + 2, thirdPixelPosition[1] - 1] = 3;
                temp[fourthPixelPosition[0] + 3, fourthPixelPosition[1]] = 4;

                firstPixelPosition = Check(1, temp);
                secondPixelPosition = Check(2, temp);
                thirdPixelPosition = Check(3, temp);
                fourthPixelPosition = Check(4, temp);
                versionElement = 0;
            }
        }
    }
    else if (type == 2)
    {
        if (versionElement == 0)
        {
            if (fourthPixelPosition[1] + 2 < temp.GetLength(1))
            {
                temp[firstPixelPosition[0], firstPixelPosition[1]] = 0;
                temp[secondPixelPosition[0], secondPixelPosition[1]] = 0;
                temp[thirdPixelPosition[0], thirdPixelPosition[1]] = 0;
                temp[fourthPixelPosition[0], fourthPixelPosition[1]] = 0;

                temp[firstPixelPosition[0] + 1, firstPixelPosition[1] - 1] = 1;
                temp[secondPixelPosition[0] + 2, secondPixelPosition[1]] = 2;
                temp[thirdPixelPosition[0] + 1, thirdPixelPosition[1] + 1] = 3;
                temp[fourthPixelPosition[0], fourthPixelPosition[1] + 2] = 4;

                firstPixelPosition = Check(1, temp);
                secondPixelPosition = Check(2, temp);
                thirdPixelPosition = Check(3, temp);
                fourthPixelPosition = Check(4, temp);
                versionElement = 1;
            }

        }
        else if (versionElement == 1)
        {
            if (fourthPixelPosition[0] - 2 >= 0)
            {
                temp[firstPixelPosition[0], firstPixelPosition[1]] = 0;
                temp[secondPixelPosition[0], secondPixelPosition[1]] = 0;
                temp[thirdPixelPosition[0], thirdPixelPosition[1]] = 0;
                temp[fourthPixelPosition[0], fourthPixelPosition[1]] = 0;

                temp[firstPixelPosition[0] + 1, firstPixelPosition[1] + 1] = 1;
                temp[secondPixelPosition[0], secondPixelPosition[1] + 2] = 2;
                temp[thirdPixelPosition[0] - 1, thirdPixelPosition[1] + 1] = 3;
                temp[fourthPixelPosition[0] - 2, fourthPixelPosition[1]] = 4;

                firstPixelPosition = Check(1, temp);
                secondPixelPosition = Check(2, temp);
                thirdPixelPosition = Check(3, temp);
                fourthPixelPosition = Check(4, temp);
                versionElement = 2;
            }

        }
        else if (versionElement == 2)
        {
            temp[firstPixelPosition[0], firstPixelPosition[1]] = 0;
            temp[secondPixelPosition[0], secondPixelPosition[1]] = 0;
            temp[thirdPixelPosition[0], thirdPixelPosition[1]] = 0;
            temp[fourthPixelPosition[0], fourthPixelPosition[1]] = 0;

            temp[firstPixelPosition[0] - 1, firstPixelPosition[1] + 1] = 1;
            temp[secondPixelPosition[0] - 2, secondPixelPosition[1]] = 2;
            temp[thirdPixelPosition[0] - 1, thirdPixelPosition[1] - 1] = 3;
            temp[fourthPixelPosition[0], fourthPixelPosition[1] - 2] = 4;

            firstPixelPosition = Check(1, temp);
            secondPixelPosition = Check(2, temp);
            thirdPixelPosition = Check(3, temp);
            fourthPixelPosition = Check(4, temp);
            versionElement = 3;
        }
        else
        {
            if (fourthPixelPosition[0] + 2 < temp.GetLength(0))
            {
                temp[firstPixelPosition[0], firstPixelPosition[1]] = 0;
                temp[secondPixelPosition[0], secondPixelPosition[1]] = 0;
                temp[thirdPixelPosition[0], thirdPixelPosition[1]] = 0;
                temp[fourthPixelPosition[0], fourthPixelPosition[1]] = 0;

                temp[firstPixelPosition[0] - 1, firstPixelPosition[1] - 1] = 1;
                temp[secondPixelPosition[0], secondPixelPosition[1] - 2] = 2;
                temp[thirdPixelPosition[0] + 1, thirdPixelPosition[1] - 1] = 3;
                temp[fourthPixelPosition[0] + 2, fourthPixelPosition[1]] = 4;

                firstPixelPosition = Check(1, temp);
                secondPixelPosition = Check(2, temp);
                thirdPixelPosition = Check(3, temp);
                fourthPixelPosition = Check(4, temp);
                versionElement = 0;
            }
        }
    }
    else if (type == 3)
    {
        if (versionElement == 0)
        {
            if (fourthPixelPosition[1] + 1 < temp.GetLength(1))
            {
                temp[firstPixelPosition[0], firstPixelPosition[1]] = 0;
                temp[secondPixelPosition[0], secondPixelPosition[1]] = 0;
                temp[thirdPixelPosition[0], thirdPixelPosition[1]] = 0;
                temp[fourthPixelPosition[0], fourthPixelPosition[1]] = 0;

                temp[firstPixelPosition[0] + 2, firstPixelPosition[1]] = 1;
                temp[secondPixelPosition[0] + 1, secondPixelPosition[1] + 1] = 2;
                temp[thirdPixelPosition[0], thirdPixelPosition[1] + 2] = 3;
                temp[fourthPixelPosition[0] - 1, fourthPixelPosition[1] + 1] = 4;

                firstPixelPosition = Check(1, temp);
                secondPixelPosition = Check(2, temp);
                thirdPixelPosition = Check(3, temp);
                fourthPixelPosition = Check(4, temp);
                versionElement = 1;
            }

        }
        else if (versionElement == 1)
        {
            if (fourthPixelPosition[0] - 1 >= 0)
            {
                temp[firstPixelPosition[0], firstPixelPosition[1]] = 0;
                temp[secondPixelPosition[0], secondPixelPosition[1]] = 0;
                temp[thirdPixelPosition[0], thirdPixelPosition[1]] = 0;
                temp[fourthPixelPosition[0], fourthPixelPosition[1]] = 0;

                temp[firstPixelPosition[0], firstPixelPosition[1] + 2] = 1;
                temp[secondPixelPosition[0] - 1, secondPixelPosition[1] + 1] = 2;
                temp[thirdPixelPosition[0] - 2, thirdPixelPosition[1]] = 3;
                temp[fourthPixelPosition[0] - 1, fourthPixelPosition[1] - 1] = 4;

                firstPixelPosition = Check(1, temp);
                secondPixelPosition = Check(2, temp);
                thirdPixelPosition = Check(3, temp);
                fourthPixelPosition = Check(4, temp);
                versionElement = 2;
            }

        }
        else if (versionElement == 2)
        {
            temp[firstPixelPosition[0], firstPixelPosition[1]] = 0;
            temp[secondPixelPosition[0], secondPixelPosition[1]] = 0;
            temp[thirdPixelPosition[0], thirdPixelPosition[1]] = 0;
            temp[fourthPixelPosition[0], fourthPixelPosition[1]] = 0;

            temp[firstPixelPosition[0] - 1, firstPixelPosition[1]] = 1;
            temp[secondPixelPosition[0], secondPixelPosition[1] - 1] = 2;
            temp[thirdPixelPosition[0] + 1, thirdPixelPosition[1] - 2] = 3;
            temp[fourthPixelPosition[0] + 2, fourthPixelPosition[1] - 1] = 4;

            firstPixelPosition = Check(1, temp);
            secondPixelPosition = Check(2, temp);
            thirdPixelPosition = Check(3, temp);
            fourthPixelPosition = Check(4, temp);
            versionElement = 3;
        }
        else
        {
            if (firstPixelPosition[0] - 1 >= 0)
            {
                temp[firstPixelPosition[0], firstPixelPosition[1]] = 0;
                temp[secondPixelPosition[0], secondPixelPosition[1]] = 0;
                temp[thirdPixelPosition[0], thirdPixelPosition[1]] = 0;
                temp[fourthPixelPosition[0], fourthPixelPosition[1]] = 0;

                temp[firstPixelPosition[0] - 1, firstPixelPosition[1] - 2] = 1;
                temp[secondPixelPosition[0], secondPixelPosition[1] - 1] = 2;
                temp[thirdPixelPosition[0] + 1, thirdPixelPosition[1]] = 3;
                temp[fourthPixelPosition[0], fourthPixelPosition[1] + 1] = 4;

                firstPixelPosition = Check(1, temp);
                secondPixelPosition = Check(2, temp);
                thirdPixelPosition = Check(3, temp);
                fourthPixelPosition = Check(4, temp);
                versionElement = 0;
            }
        }
    }
    else if (type == 4)
    {
        if (versionElement == 0)
        {
            if (thirdPixelPosition[1] + 2 < temp.GetLength(1))
            {
                temp[firstPixelPosition[0], firstPixelPosition[1]] = 0;
                temp[secondPixelPosition[0], secondPixelPosition[1]] = 0;
                temp[thirdPixelPosition[0], thirdPixelPosition[1]] = 0;
                temp[fourthPixelPosition[0], fourthPixelPosition[1]] = 0;

                temp[firstPixelPosition[0] + 2, firstPixelPosition[1]] = 1;
                temp[secondPixelPosition[0] + 1, secondPixelPosition[1] + 1] = 2;
                temp[thirdPixelPosition[0], thirdPixelPosition[1] + 2] = 3;
                temp[fourthPixelPosition[0], fourthPixelPosition[1]] = 4;

                firstPixelPosition = Check(1, temp);
                secondPixelPosition = Check(2, temp);
                thirdPixelPosition = Check(3, temp);
                fourthPixelPosition = Check(4, temp);
                versionElement = 1;
            }

        }
        else if (versionElement == 1)
        {
            if (thirdPixelPosition[0] - 2 >= 0)
            {
                temp[firstPixelPosition[0], firstPixelPosition[1]] = 0;
                temp[secondPixelPosition[0], secondPixelPosition[1]] = 0;
                temp[thirdPixelPosition[0], thirdPixelPosition[1]] = 0;
                temp[fourthPixelPosition[0], fourthPixelPosition[1]] = 0;

                temp[firstPixelPosition[0], firstPixelPosition[1] + 2] = 1;
                temp[secondPixelPosition[0] - 1, secondPixelPosition[1] + 1] = 2;
                temp[thirdPixelPosition[0] - 2, thirdPixelPosition[1]] = 3;
                temp[fourthPixelPosition[0], fourthPixelPosition[1]] = 4;

                firstPixelPosition = Check(1, temp);
                secondPixelPosition = Check(2, temp);
                thirdPixelPosition = Check(3, temp);
                fourthPixelPosition = Check(4, temp);
                versionElement = 2;
            }

        }
        else if (versionElement == 2)
        {
            if (firstPixelPosition[1] + 1 < temp.GetLength(1))
            {
                temp[firstPixelPosition[0], firstPixelPosition[1]] = 0;
                temp[secondPixelPosition[0], secondPixelPosition[1]] = 0;
                temp[thirdPixelPosition[0], thirdPixelPosition[1]] = 0;
                temp[fourthPixelPosition[0], fourthPixelPosition[1]] = 0;

                temp[firstPixelPosition[0] - 2, firstPixelPosition[1]] = 1;
                temp[secondPixelPosition[0] - 1, secondPixelPosition[1] - 1] = 2;
                temp[thirdPixelPosition[0], thirdPixelPosition[1] - 2] = 3;
                temp[fourthPixelPosition[0], fourthPixelPosition[1]] = 4;

                firstPixelPosition = Check(1, temp);
                secondPixelPosition = Check(2, temp);
                thirdPixelPosition = Check(3, temp);
                fourthPixelPosition = Check(4, temp);
                versionElement = 3;
            }
        }
        else
        {
            if (thirdPixelPosition[0] + 2 < temp.GetLength(0))
            {
                temp[firstPixelPosition[0], firstPixelPosition[1]] = 0;
                temp[secondPixelPosition[0], secondPixelPosition[1]] = 0;
                temp[thirdPixelPosition[0], thirdPixelPosition[1]] = 0;
                temp[fourthPixelPosition[0], fourthPixelPosition[1]] = 0;

                temp[firstPixelPosition[0], firstPixelPosition[1] - 2] = 1;
                temp[secondPixelPosition[0] + 1, secondPixelPosition[1] - 1] = 2;
                temp[thirdPixelPosition[0] + 2, thirdPixelPosition[1]] = 3;
                temp[fourthPixelPosition[0], fourthPixelPosition[1]] = 4;

                firstPixelPosition = Check(1, temp);
                secondPixelPosition = Check(2, temp);
                thirdPixelPosition = Check(3, temp);
                fourthPixelPosition = Check(4, temp);
                versionElement = 0;
            }
        }
    }
    else if (type == 5)
    {
        if (versionElement == 0)
        {
            if (fourthPixelPosition[1] + 1 < temp.GetLength(1))
            {
                temp[firstPixelPosition[0], firstPixelPosition[1]] = 0;
                temp[secondPixelPosition[0], secondPixelPosition[1]] = 0;
                temp[thirdPixelPosition[0], thirdPixelPosition[1]] = 0;
                temp[fourthPixelPosition[0], fourthPixelPosition[1]] = 0;

                temp[firstPixelPosition[0] + 1, firstPixelPosition[1]] = 1;
                temp[secondPixelPosition[0], secondPixelPosition[1] + 1] = 2;
                temp[thirdPixelPosition[0] - 1, thirdPixelPosition[1]] = 3;
                temp[fourthPixelPosition[0] - 2, fourthPixelPosition[1] + 1] = 4;

                firstPixelPosition = Check(1, temp);
                secondPixelPosition = Check(2, temp);
                thirdPixelPosition = Check(3, temp);
                fourthPixelPosition = Check(4, temp);
                versionElement = 1;
            }

        }
        else if (versionElement == 1)
        {
            if (fourthPixelPosition[0] +2 < temp.GetLength(0))
            {
                temp[firstPixelPosition[0], firstPixelPosition[1]] = 0;
                temp[secondPixelPosition[0], secondPixelPosition[1]] = 0;
                temp[thirdPixelPosition[0], thirdPixelPosition[1]] = 0;
                temp[fourthPixelPosition[0], fourthPixelPosition[1]] = 0;

                temp[firstPixelPosition[0] - 1, firstPixelPosition[1]] = 1;
                temp[secondPixelPosition[0], secondPixelPosition[1] - 1] = 2;
                temp[thirdPixelPosition[0] + 1, thirdPixelPosition[1]] = 3;
                temp[fourthPixelPosition[0] + 2, fourthPixelPosition[1] - 1] = 4;

                firstPixelPosition = Check(1, temp);
                secondPixelPosition = Check(2, temp);
                thirdPixelPosition = Check(3, temp);
                fourthPixelPosition = Check(4, temp);
                versionElement = 0;
            }

        }
        
    }
    else
    {
        if (versionElement == 0)
        {
            if (fourthPixelPosition[1] + 2 < temp.GetLength(1))
            {
                temp[firstPixelPosition[0], firstPixelPosition[1]] = 0;
                temp[secondPixelPosition[0], secondPixelPosition[1]] = 0;
                temp[thirdPixelPosition[0], thirdPixelPosition[1]] = 0;
                temp[fourthPixelPosition[0], fourthPixelPosition[1]] = 0;

                temp[firstPixelPosition[0], firstPixelPosition[1] - 1] = 1;
                temp[secondPixelPosition[0] - 1, secondPixelPosition[1]] = 2;
                temp[thirdPixelPosition[0], thirdPixelPosition[1] + 1] = 3;
                temp[fourthPixelPosition[0] - 1, fourthPixelPosition[1] + 2] = 4;

                firstPixelPosition = Check(1, temp);
                secondPixelPosition = Check(2, temp);
                thirdPixelPosition = Check(3, temp);
                fourthPixelPosition = Check(4, temp);
                versionElement = 1;
            }

        }
        else if (versionElement == 1)
        {
            if (fourthPixelPosition[0] + 1 < temp.GetLength(0))
            {
                temp[firstPixelPosition[0], firstPixelPosition[1]] = 0;
                temp[secondPixelPosition[0], secondPixelPosition[1]] = 0;
                temp[thirdPixelPosition[0], thirdPixelPosition[1]] = 0;
                temp[fourthPixelPosition[0], fourthPixelPosition[1]] = 0;

                temp[firstPixelPosition[0], firstPixelPosition[1] + 1] = 1;
                temp[secondPixelPosition[0] + 1, secondPixelPosition[1]] = 2;
                temp[thirdPixelPosition[0], thirdPixelPosition[1] - 1] = 3;
                temp[fourthPixelPosition[0] + 1, fourthPixelPosition[1] - 2] = 4;

                firstPixelPosition = Check(1, temp);
                secondPixelPosition = Check(2, temp);
                thirdPixelPosition = Check(3, temp);
                fourthPixelPosition = Check(4, temp);
                versionElement = 0;
            }

        }

    }
}

void ClearOldPixel()
{
    map[firstPixelPosition[0], firstPixelPosition[1]] = 0;
    map[secondPixelPosition[0], secondPixelPosition[1]] = 0;
    map[thirdPixelPosition[0], thirdPixelPosition[1]] = 0;
    map[fourthPixelPosition[0], fourthPixelPosition[1]] = 0;
}
int[] Check(int number, int[,] array) // метод для определения координат пикселя в массиве currentPosition
{
    int[] result = new int[2];
    for (int i = 0; i < 10; i++) // цикл для поиска в масиве нужного числа 
    {
        for (var j = 0; j < 20; j++)
        {
            if (number == array[i, j])
            {
                result[0] = i;
                result[1] = j;
            }
        }
    }
    return result;
}
