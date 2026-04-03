const ORCHESTRATOR_URL = "http://localhost:5000";

const readout = document.getElementById("readout");
let isLoading = false;

async function handleClick(count) {
  if (isLoading) return;

  isLoading = true;
  updateUI("loading");

  try {
    const response = await fetch(`${ORCHESTRATOR_URL}/result?count=${count}`);

    if (!response.ok) {
      throw new Error(`HTTP ${response.status}`);
    }

    const data = await response.json();
    updateUI("result", data.total);
  } catch (error) {
    updateUI("error", error.message);
  } finally {
    isLoading = false;
    document.querySelectorAll("button").forEach(btn => {
      btn.disabled = false;
    });
  }
}

function updateUI(state, value) {
  const buttons = document.querySelectorAll("#buttons button");

  readout.className = state;

  switch (state) {
    case "loading":
      buttons.forEach(btn => btn.disabled = true);
      readout.textContent = "Loading...";
      break;
    case "result":
      readout.textContent = value;
      buttons.forEach(btn => btn.disabled = false);
      break;
    case "error":
      readout.textContent = `Error: ${value}`;
      buttons.forEach(btn => btn.disabled = false);
      break;
  }
}
