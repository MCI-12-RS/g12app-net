using G12App.Domain.Interfaces.Services;
using G12App.Domain.Model;
using System.Globalization;

namespace G12App.Application.Services
{
    public class ColumnFormatter : IColumnFormatter
    {
        public IDictionary<string, Func<CellReports, object>> GetCellReportFormatter()
        {
            return new Dictionary<string, Func<CellReports, object>>
            {
                { "Célula #", (x) => x.Cell.Id },
                { "G12", (x) => x.Cell.Lead.ComputedG12Leader },
                { "Líder", (x) => x.Cell.Lead.Name },
                { "Timóteo", (x) => x.Cell.Timothy.Name },
                { "Anfitrião", (x) => x.Cell.Host.Name },
                { "Endereço", (x) => !string.IsNullOrWhiteSpace(x.Cell.Place.Street) ?
                                    x.Cell.Place.Street + (!string.IsNullOrWhiteSpace(x.Cell.Place.Number) ? ", " + x.Cell.Place.Number : "")
                                    : "" },
                { "Bairro", (x) => x.Cell.Place.Neighborhood },
                { "Situação", (x) => x.Reports.LastOrDefault() == null ?
                                        "Relatório não entregue." :
                                        x.Reports.LastOrDefault().WeekCellSkipped ?
                                        "Não houve célula." :
                                        "Célula realizada." },
                { "Data / Hora", (x) => x.Reports.LastOrDefault() != null && !x.Reports.LastOrDefault().WeekCellSkipped ?
                                         x.Reports.LastOrDefault().Date.ToString("dd/MM/yyyy HH:mm") : string.Empty },
                { "Oferta", (x) => x.Reports.LastOrDefault()?.Contribution.ToString("C2", CultureInfo.GetCultureInfo("pt-BR")) },
                { "Numero Participantes", (x) => x.Reports.LastOrDefault()?.NumMembers },
                { "Participantes", (x) => x.Reports.LastOrDefault()?.Members
                                    .Aggregate("", (agg, item) => agg + (!string.IsNullOrWhiteSpace(agg) ? ", " : "" ) + item.Name) },
                { "Numero Visitantes", (x) => x.Reports.LastOrDefault()?.NumVisitors },
                { "Observações", (x) => x.Reports.LastOrDefault()?.Notes },
            };
        }
    }
}
