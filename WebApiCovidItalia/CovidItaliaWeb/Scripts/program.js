function drawChartPositivi() {
    $.ajax("https://localhost:44355/covid")
        .done(function (jsondata) {
            var dsLabels = jsondata.map(function (jd) {
                return jd.data;
            });

            var dsNuoviPositivi = jsondata.map(function (jd) {
                return jd.nuovi_positivi;
            });

            var dsVarPositivi = jsondata.map(function (jd) {
                return jd.variazione_totale_positivi;
            });

            var dsTotalePositivi = jsondata.map(function (jd) {
                return jd.totale_positivi;
            });

            new Chart($('#chartPositivi'), {
                type: 'line',
                data: {
                    labels: dsLabels,
                    datasets: [{
                        label: 'Nuovi positivi',
                        fill: false,
                        backgroundColor: window.chartColors.red,
                        borderColor: window.chartColors.red,
                        data: dsNuoviPositivi
                    }, {
                        label: 'Variazione positivi',
                        fill: false,
                        backgroundColor: window.chartColors.orange,
                        borderColor: window.chartColors.orange,
                        data: dsVarPositivi
                    }]
                },
                options: {
                    responsive: true,
                    parsing: false,
                    title: {
                        display: true,
                        text: 'Andamento Positivi'
                    },
                    tooltips: {
                        mode: 'index',
                        intersect: false,
                    },
                    hover: {
                        mode: 'nearest',
                        intersect: true
                    },
                    scales: {
                        xAxes: [{
                            display: true,
                            scaleLabel: {
                                display: false,
                                labelString: 'Giorni'
                            }
                        }],
                        yAxes: [{
                            display: true,
                            scaleLabel: {
                                display: false,
                                labelString: 'Numero'
                            }
                        }]
                    }
                }
            });
        })
        .fail(function () {
            alert("error");
        });
}