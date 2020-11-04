Template Scene -> SceneManagers  
В SceneManagers висят обязательные Observer и LevelManager.  
Observer является связующим компонентом между игровыми сущностями и\или интерфейсом со списком всех событий в игре.  

 - Основные события :  
OnGameStarted срабатывает по нажатию кнопки-туториала (описано ниже).  
OnWinLevel событие, которое необходимо вызывать при финише\выигрыша сцены.  
После вызова вылетает панель победы (и другое в зависимости от подписок).  
OnLoseLevel событие, которое соответственно необходимо вызывать при проигрыше.  
После вызова вылетает панель проигрыша (и другое в зависимости от подписок).  

 - Вызов событий :  
Observer.Instance.OnWinLevel();  
Observer.Instance.CallOnWinLevel(); - метод исключающий множественные вызовы события OnWinLevel;  
Observer.Instance.OnLoseLevel();  
Observer.Instance.CallOnLoseLevel();  

 - Подписка на события :  
Observer.Instance.OnWinLevel += SomeMethod;  
Observer.Instance.OnWinLevel += delegate { SomeMethod(2f); };  

 - В сцене висит Canvas с панелью победы \ проигрыша и кнопкой-туториалом (рука + восьмерка \ тап ту плей).  
Кнопка-туториал растянута на весь экран и по нажатию отключается и вызывает событие OnGameStarted.  
В панели уже настроены кнопки рестарта сцены и загрузки следующей сцены.  

 - Все уровни (все сцены имеющиеся в build settings) автоматически зациклены.  
При каждом старте сцены загружается последняя непройденная сцена.  
Чтобы поменять загружаемую при старте сцену нужно в LevelManager вписать индекс нужной сцены и нажать Set current scene.  
Чтобы сбросить прогресс до нулевой сцены нужно нажать Reset progress.  

 - В GameConstants в идеале пишутся все константы игры как строковые так и численные, чтобы к ключевым константам игры был доступ без поиска в коде.  

Дополнительные шаблоны - Assets/Templates

 - ProgressBars Templates  
В папке Assets/Templates/ProgressBars Templates есть примеры  
Подробное описание по ссылке https://github.com/KonstantKuz/ProgressBars-Templates  
Плюшки - canvas\world space прогресс\стейдж бары с легкой сменой направления независимо от роста\убавления прогресса  

 - ObjectPooler  
В папке Assets/Templates/ObjectPooler/Example есть пример  
Подробное описание по ссылке https://github.com/KonstantKuz/ObjectPooler  
Плюшки - спавн с автовозвратом через n сек, спавн объектов рандомно, спавн объектов со взвешенным рандомом.  

 - ScreenshotMaker  
Рантайм эдитор скриншотер, делающий скрины в разных разрешениях за раз.  
Исп. : кинуть в сцену префаб, добавить разрешения и поставить галочку MakeShot если нужен скрин в данном разрешении.  

 - Popup Text  
World space всплывашка с простенькой popup анимацией.  
Пр. ObjectPooler.Instance.SpawnObject("Popup").GetComponent<PopupText>().SetPopup("+1", Color.red);  

 - StimulText  
Canvas space всплывашка с 3 анимациями.  
  