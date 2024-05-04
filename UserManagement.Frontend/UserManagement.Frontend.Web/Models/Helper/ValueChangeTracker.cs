namespace UserManagement.Frontend.Web.Models.Helper;

public class ValueChangeTracker<TDataType, TTrackerType>
{
    public TDataType? Value { get; set; }
    public TTrackerType? Changed { get; set; }
}
