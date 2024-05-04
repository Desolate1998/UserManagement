using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Backend.Common.ValueChangeTracker;

public class ValueChangeTracker<TDataType,TTrackerType>
{
    public TDataType? Value {get; set; }
    public TTrackerType? Changed { get; set; }
}
