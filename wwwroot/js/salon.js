function displayAlert(eleSelector) {
  var myAlert = document.querySelector(eleSelector);
  if (!myAlert) return;

  myAlert.classList.add("show");
}

function closeAlert(eleSelector) {
  var myAlert = document.querySelector(eleSelector);
  if (!myAlert) return;

  const alert = bootstrap.Alert.getOrCreateInstance(myAlert);
  alert.close();
}
