function drawChart() {
    $.ajax("https://localhost:44355/covid")
        .done(function (jsondata) {
            var lbl = jsondata.map(function (jd) {
                return jd.data;
            });

            var nuoviPositivi = jsondata.map(function (jd) {
                return jd.nuovi_positivi;
            });

            var varPositivi = jsondata.map(function (jd) {
                return jd.variazione_totale_positivi;
            });

            var totalePositivi = jsondata.map(function (jd) {
                return jd.totale_positivi;
            });

            new Chart($('#chart1'), {
                type: 'line',
                data: {
                    labels: lbl,
                    datasets: [{
                        label: 'Nuovi positivi',
                        fill: false,
                        backgroundColor: window.chartColors.red,
                        borderColor: window.chartColors.red,
                        data: nuoviPositivi
                    }, {
                        label: 'Variazione positivi',
                        fill: false,
                        backgroundColor: window.chartColors.orange,
                        borderColor: window.chartColors.orange,
                        data: varPositivi
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