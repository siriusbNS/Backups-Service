# Backup-Service


### Пример логики работы

Выполняем следующие действия:

1. Имеем в **Repository** три объекта `File A`, `File B`, `Folder C`.
2. Создаём **Backup Task**, добавляем в неё три **Backup Object**, соответсвующие объектам находящимся в репозитории.
3. Запускаем выполнение **Backup Task**, получаем **Restore Point**, он записывается в репозиторий, в соответствующей директории появляются **Storage** `File A(1)`, `File B(1)`, `Folder C(1)`.
4. Запускаем выполнение ещё раз, получаем **Storage** с версиями `(2)`.
5. Удаляем из **Backup Task** `File B`, запускаем выполнение ещё раз, получаем третий **Restore Point**, ему будут соответствовать два **Storage** - `File A(3)`, `Folder C(3)`.

### Создание резервных копий

Под созданием резервной копии данных, подразумевается создание копии данных в другом месте. Система поддерживает расширяемость в вопросе выбора **Storage Algorithm**, используемых для хранения резервных копий (должна иметь возможность добавить новый алгоритм безболезненно, помним про OCP). 

В данной лабораторной требуется реализовать два **Storage Algorithm**:

1. **Split Storage** – алгоритм раздельного хранения, для каждого **Backup Object** в **Restore Point** создаётся отдельный **Storage** - архив, в котором лежат данные объекта.
2. **Single Storage** – алгоритм общего хранения, для всех **Backup Object** в **Restore Point** создаётся один общий **Storage** - архив, в котором лежат данные каждого объекта.

**Storage Algorithm** не должен нести ответственность за реализацию архивации. 

### Хранение копий

Резервные копии могут сохранятся как на локальной файловой системе, так и в ОЗУ.

В тестах стоит реализованно хранение в памяти (**InMemoryRepository**), так как при запуске тестов на настоящей файловой системе будет генерироваться много мусорных данных, а так же системы CI не дружат с запросами к файловой системе во время автоматического выполнения тестов.

Структура:

- Корневая директория
    - Директории различных **Backup Task**
        - Директории различных **Restore Point**
            - Файлы **Storage**

### Создание Restore Point

**Backup Task** отвечает за создание новых точек восстановления, выступает фасадом, инкапсулируя логику выполнения этой операции. 

При создании **Backup Task** есть возможность указать её название, **Repository** для хранения **Backup** (его данных), **Storage Algorithm**.

**Backup Task** поддерживает операции добавления и удаления отслеживаемых ей **Backup Object**.

Результатом выполнения **Backup Task** является создание **Restore Point** и соответствующих ей **Storage** в выбранном **Repository**.

### Тест кейсы

1. Тест 1
    1. Создаём **Backup Task**, использующую **Split Storage**
    2. Добавляем в **Backup Task** два **Backup Object**
    3. Запускаем выполнение **Backup Task**
    4. Удаляем из **Backup Task** один **Backup Object**
    5. Запускаем выполнение **Backup Task**
    6. Проверяем то, что было создано две **Restore Point** и три **Storage**
2. Тест 2 (лучше оформить в виде консольного приложения, так как нормально проверить можно только на настоящей файловой системе)
    1. Создаём **Backup Task**, использующую **FileSystemRepository** и **Single Storage**
    2. Добавляем в **Backup Task** два **Backup Object**
    3. Запускаем выполнение **Backup Task**
    4. Проверяем то, что директории и файлы были созданы
