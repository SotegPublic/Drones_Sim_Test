using UnityEngine.AI;

public class DrawDronePathController : IUpdatableController
{
    private IDronesHolder _dronesHolder;
    private IMainUINotifier _mainUINotifier;

    private bool _isDrawPath;

    public DrawDronePathController(IDronesHolder dronesHolder, IMainUINotifier mainUINotifier, GameConfig gameConfig)
    {
        _dronesHolder = dronesHolder;
        _mainUINotifier = mainUINotifier;

        _isDrawPath = gameConfig.IsDrawPath;
        _mainUINotifier.OnPathToggleChange += SwitchDrawing;
    }

    private void SwitchDrawing(bool isDrawPath)
    {
        _isDrawPath = isDrawPath;

        if (!_isDrawPath)
            EraseLines();
    }

    private void EraseLines()
    {
        foreach (var dronesList in _dronesHolder.Drones.Values)
        {
            for (int i = 0; i < dronesList.Count; i++)
            {
                dronesList[i].View.LineRenderer.positionCount = 0;
            }
        }
    }

    public void Update()
    {
        if (!_isDrawPath)
            return;

        foreach(var dronesList in _dronesHolder.Drones.Values)
        {
            for(int i = 0; i < dronesList.Count; i++)
            {
                var drone = dronesList[i];
                DrawDronePath(drone);
            }
        }
    }

    private void DrawDronePath(DroneModel drone)
    {
        if (drone.View.Agent == null || drone.View.LineRenderer == null)
            return;

        NavMeshPath path = new NavMeshPath();
        if (drone.View.Agent.CalculatePath(drone.View.Agent.destination, path))
        {
            drone.View.LineRenderer.positionCount = path.corners.Length;
            drone.View.LineRenderer.SetPositions(path.corners);
        }
    }
}
