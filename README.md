# Drones_Sim_Test v0.0.3 (Updated - доработана система таргетинга)

Для реализации использоовались Zenject, UniTask и DOTween (ради целой одной анимации возврата ресурса).
- [FSM](https://github.com/SotegPublic/Drones_Sim_Test/tree/main/Assets/Scripts/Infrastructure/GameStateMachine) - стейт машина, управляющая состояниями симуляции
- [Drones Controller](https://github.com/SotegPublic/Drones_Sim_Test/blob/main/Assets/Scripts/Drones/DronesController.cs) - контроллер управления дронами, управление реализовано через систему состояний дрона (упрощенная стейт машина).
- [Avoidance](https://github.com/SotegPublic/Drones_Sim_Test/blob/main/Assets/Scripts/Drones/DronesAvoidanceSystem.cs) - система избегания столкновений
- [Resources controller](https://github.com/SotegPublic/Drones_Sim_Test/blob/main/Assets/Scripts/ResourcesFolder/ResourcesController.cs) - система управления ресурсами (спавн, удаление). Спавн ресурсов происходит по гриду, который создается на этапе [инициализации](https://github.com/SotegPublic/Drones_Sim_Test/blob/main/Assets/Scripts/Infrastructure/GameStateMachine/States/GamePreparationState.cs) симуляции
- [Pool System](https://github.com/SotegPublic/Drones_Sim_Test/tree/main/Assets/Scripts/Infrastructure/Pool) - для спавна дронов и ресурсов используется система пулинга
- [Path Drawler](https://github.com/SotegPublic/Drones_Sim_Test/blob/main/Assets/Scripts/Drones/DrawDronePathController.cs) - для отрисовки пути используются Line Renderer на каждом агенте, которыми сверху управляет глобальный контроллер.
- [UI View](https://github.com/SotegPublic/Drones_Sim_Test/blob/main/Assets/Scripts/UI/Views/MainUIView.cs) и [UI Controller](https://github.com/SotegPublic/Drones_Sim_Test/blob/main/Assets/Scripts/UI/Controllers/MainUIController.cs) - Система UI.

  Задачу такого типа делал первый раз, поэтому реализация потребовала куда больше времени) Кроме того есть момент, которые хотелось бы изменить, но на это опять же нужно время:
  - Спавн дронов, по хорошему, это должна быть очередь последовательных операций спавна с возможностью отмены очереди через канселейшен токен, в случае если игрок изменит количество дронов в меньшую сторону. Сейчас, в любом случае, все работает, но так было бы красивее.
 
  По дополнительным задачам, опять же вопрос времени по большей части:
  - Текущее состояние дрона легко отобразить, так как дроны работают через "стейты", просто пара строк кода в методы смены состояния.
  - Управление скоростью симуляции. Из самого простого еще один слайдер влияющий на таймскейл. Но мне кажется лучше просто глобальный модификатор, который будет влиять на скорость движения дронов, скорость сбора ресурса, скорость спавна ресурсов.
  - Выбор дрона для слежения. Я уже подключил инпут, просто делать рейкаст в сцену, ловить дрона и в лейтапдейте двигать камеру за дроном по X координате.
  - Миникарта сложнее всего что выше, так как делал один раз и давно) Если верно помню, текстура, доп камера и проецирование объектов сцены на эту текстуру.
