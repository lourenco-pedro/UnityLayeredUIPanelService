namespace ppl.ServiceManagement.LayeredUIService
{
    public interface ILayeredUIService : IService
    {
        int UseCanvas(ILayeredPanel ipanel, BaseLayeredPanel.EntranceType entranceType = BaseLayeredPanel.EntranceType.Instant);
        int UseHigherCanvas(ILayeredPanel ipanel);
        BaseLayeredPanel GetPanel(int panelId);
        void Close(int panelId);
    }
}