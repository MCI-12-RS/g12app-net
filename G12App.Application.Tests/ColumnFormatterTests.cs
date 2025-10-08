using System;
using System.Collections.Generic;
using System.Globalization;
using FluentAssertions;
using G12App.Application.Services;
using G12App.Domain.Model;

namespace G12App.Application.Tests
{
    public class ColumnFormatterTests
    {
        [Fact]
        public void GetCellReportFormatter_ShouldFormatLatestReportDetails()
        {
            var formatter = new ColumnFormatter();
            var formatters = formatter.GetCellReportFormatter();
            var reportDate = new DateTime(2024, 2, 3, 19, 30, 0);

            var data = new CellReports
            {
                Cell = new Cell
                {
                    Id = 10,
                    Lead = new Member
                    {
                        Name = "Carlos Henrique da Silva",
                        ComputedG12Leader = "Maria Aparecida de Souza"
                    },
                    Place = new Address
                    {
                        Street = "Rua das Flores",
                        Number = "123",
                        Neighborhood = "Centro"
                    }
                },
                Reports = new List<Report>
                {
                    new Report
                    {
                        Date = reportDate,
                        Contribution = 123.45m,
                        NumMembers = 15,
                        NumVisitors = 4,
                        Notes = "Reunião especial",
                        WeekCellSkipped = false
                    }
                }
            };

            formatters["Célula #"](data).Should().Be(10);
            formatters["G12"](data).Should().Be("Maria Souza");
            formatters["Líder"](data).Should().Be("Carlos Silva");
            formatters["Endereço"](data).Should().Be("Rua das Flores, 123");
            formatters["Bairro"](data).Should().Be("Centro");
            formatters["Situação"](data).Should().Be("Célula realizada.");
            formatters["Data / Hora"](data).Should().Be(reportDate.ToString("dd/MM/yyyy HH:mm"));
            formatters["Oferta"](data).Should().Be(123.45m.ToString("C2", CultureInfo.GetCultureInfo("pt-BR")));
            formatters["Numero Participantes"](data).Should().Be(15);
            formatters["Numero Visitantes / Novos"](data).Should().Be(4);
            formatters["Observações"](data).Should().Be("Reunião especial");
        }

        [Fact]
        public void GetCellReportFormatter_ShouldHandleMissingInformation()
        {
            var formatter = new ColumnFormatter();
            var formatters = formatter.GetCellReportFormatter();

            var data = new CellReports
            {
                Cell = new Cell
                {
                    Id = 5,
                    Lead = new Member
                    {
                        Name = "Joao",
                        ComputedG12Leader = string.Empty
                    },
                    Place = new Address()
                },
                Reports = new List<Report>()
            };

            formatters["G12"](data).Should().Be("Joao Joao");
            formatters["Endereço"](data).Should().Be(string.Empty);
            formatters["Situação"](data).Should().Be("Relatório não entregue.");
            formatters["Data / Hora"](data).Should().Be(string.Empty);
            formatters["Oferta"](data).Should().BeNull();
            formatters["Numero Participantes"](data).Should().BeNull();
            formatters["Numero Visitantes / Novos"](data).Should().BeNull();
            formatters["Observações"](data).Should().BeNull();
        }
    }
}
