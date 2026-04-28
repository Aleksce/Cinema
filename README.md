# LumiSeat (WPF + SQLite + TMDB)

Приложение онлайн-бронирования билетов в кинотеатр для дипломной работы.

## Какая БД используется и как к ней получить доступ

Используется **SQLite**. База хранится в файле **`cinema.db`** рядом с исполняемым файлом приложения (`bin/...`).

Открыть базу можно через:
- DB Browser for SQLite,
- DBeaver,
- DataGrip,
- или Visual Studio (Server Explorer).

## Возможности

- Регистрация/вход пользователей.
- Сеансы и места с фиксацией занятости.
- Бронирование и список «Мои билеты».
- Подтягивание текущих фильмов из **TMDB** (постер, описание, цена, расписание).
- Современный UI: карточки, градиенты, разделы, настройки темы.

## Настройка TMDB

Файл: `CinemaBooking.App/appsettings.json`

```json
{
  "Tmdb": {
    "ApiKey": "YOUR_API_KEY",
    "ReadAccessToken": "YOUR_READ_ACCESS_TOKEN"
  }
}
```

> Важно: не храните реальные ключи в git. Для продакшена лучше использовать переменные окружения.

Поддерживаются переменные окружения:
- `CINEMA_TMDB__APIKEY`
- `CINEMA_TMDB__READACCESSTOKEN`

## Архитектура

- `Models` — сущности домена.
- `Data` — EF Core `DbContext`, инициализация и seed.
- `Integrations` — клиент TMDB.
- `Services` — логика авторизации и бронирования.
- `ViewModels` — состояние и команды UI.
- `MainWindow.xaml` — интерфейс (вкладки: Главная / Аккаунт / Мои билеты / Настройки).

## Запуск

1. Откройте `CinemaBooking.sln` в Visual Studio 2022.
2. Убедитесь, что установлен workload **.NET desktop development**.
3. Выберите `CinemaBooking.App` как Startup Project.
4. Восстановите NuGet (`Restore`).
5. Запустите Build (`Ctrl+Shift+B`) и затем старт (`F5`).

## Исправление ошибки “исполняемый файл для отладки … не существует”

Если видите ошибку вроде:
`...\bin\Debug\net8.0-windows\CinemaBooking.App.exe ... не существует`,
выполните:

1. Закройте проект и откройте **именно `CinemaBooking.sln`**.
2. ПКМ по `CinemaBooking.App` → **Set as Startup Project**.
3. Удалите папки `bin` и `obj` внутри `CinemaBooking.App`.
4. `Build` → `Rebuild Solution`.
5. Если не помогло: `Debug` → `Open debug launch profiles UI` и выберите профиль **CinemaBooking.App** (command: Project).

После успешной сборки файл `CinemaBooking.App.exe` появится автоматически.
