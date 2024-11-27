using ExtendableEnums.Testing.Models;

namespace ExtendableEnums.Demo.Wasm.Pages;

public partial class Index
{
    private SampleStatus[] StatusOptions => SampleStatus.GetAll();
}