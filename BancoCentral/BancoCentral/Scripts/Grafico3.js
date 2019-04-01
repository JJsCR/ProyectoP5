var TCDCanvas = document.getElementById("TCDChart");

Chart.defaults.global.defaultFontFamily = "Lato";
Chart.defaults.global.defaultFontSize = 18;

var TCDData = {
    labels: ["2014", "2015", "2016", "2017", "2018", "2019"],
    datasets: [
        {
            label: "Tasa Basica Pasiva",
            data: [557.62, 533.31, 548.32, 574.94, 622.99, 610.01]
        }
    ]
};

var chartOption = {
    legend: {
        display: true,
        position: "top",
        labels: {
            boxWidth: 80,
            fontColor: "black"
        }
    }
};

var lineChar = new Chart(TCDCanvas, {
    type: "line",
    data: TCDData,
    options: chartOption
});