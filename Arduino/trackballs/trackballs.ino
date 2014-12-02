#include <ps2.h>


PS2 mouse(6, 5); //(clock, data)

void setup()
{
  Serial.begin(57600);
  mouse_init();
}

void loop()
{
  poll_mouse(0);
}

/*
 * initialize the mouse. Reset it, and place it into remote
 * mode, so we can get the encoder data on demand.
 */
void mouse_init()
{
  mouse.write(0xff);  // reset
  mouse.read();  // ack byte
  mouse.read();  // blank */
  mouse.read();  // blank */
  mouse.write(0xf0);  // remote mode
  mouse.read();  // ack
  delayMicroseconds(100);
}

void poll_mouse(char mouseNum)
{
  /* get a reading from the mouse */
  mouse.write(0xeb);  // give me data!
  mouse.read();      // ignore ack
  char mstat = mouse.read();
  char mx = mouse.read();
  char my = mouse.read();

  /* send the data to the Mac */
  Serial.print(":m");
  Serial.print((int)mouseNum);
  Serial.print("\t");
  Serial.print((int)mstat);
  Serial.print("\t");
  Serial.print((int)mx);
  Serial.print("\t");
  Serial.print((int)my);
  Serial.println();
  delay(20);
}
