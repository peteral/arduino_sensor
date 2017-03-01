void setup() {
	// put your setup code here, to run once:
	Serial.begin(9600);
}

void loop() {
	// put your main code here, to run repeatedly:
	int sensorVal = analogRead(A0);
	float voltage = (sensorVal / 1024.0) * 5.0;
	float temperature = (voltage - 0.5) * 100;
	Serial.println(temperature);
	delay(1000);
}