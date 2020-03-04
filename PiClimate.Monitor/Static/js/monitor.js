document.body.onload = async () =>
{
  let data = {};
  try
  {
    let response = await fetch("/data", {
      method: "POST",
      mode: "cors",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify({
        fromTime: new Date(Date.now() - 24 * 60 * 60 * 1000),
        toTime: new Date(Date.now()),
        resolution: 24 * 60 * 60
      })
    });
    let jsonData = await response.json();
    data = jsonData.measurements.map(measurement =>
    {
      return {
        x: measurement.d,
        y: measurement.p
      }
    });
  }
  catch
  {
  }

  let chartCanvas = document.getElementById("pressure-chart").getContext("2d");
  let chart = new Chart(chartCanvas,
    {
      type: "scatter",
      data: {
        datasets: [
          {
            label: "Pressure",
            data: data,
            radius: 0,
            showLine: true,
            borderColor: "blue",
            borderWidth: 2,
            lineTension: 0,
            fill: false
          }
        ],
      },
      options: {
        scales: {
          xAxes: [
            {
              type: "time",
              time: {
                minUnit: "second"
              }
            }
          ]
        },
        tooltips: {
          mode: "x",
          intersect: false
        }
      }
    }
  );
};
