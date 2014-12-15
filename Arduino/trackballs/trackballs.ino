#include <ps2.h>


PS2 mouse[3] = {PS2(2, 3), PS2(4, 5), PS2(6, 7)}; //(clock, data)

void setup()
{
  Serial.begin(57600);
  Serial.println("1");
  mouse_init(0);
  mouse_init(1);
  mouse_init(2);
}

void loop()
{
  poll_mouse(0);
  poll_mouse(1);
  poll_mouse(2);
}

/*
 * initialize the mouse. Reset it, and place it into remote
 * mode, so we can get the encoder data on demand.
 */
void mouse_init(int i)
{
  Serial.println("2");
  mouse[i].write(0xff);  // reset
  Serial.println("3");
  mouse[i].read();  // ack byte
  mouse[i].read();  // blank */
  mouse[i].read();  // blank */
  mouse[i].write(0xf0);  // remote mode
  mouse[i].read();  // ack
  delayMicroseconds(100);
}

void poll_mouse(int i)
{
  /* get a reading from the mouse */
  mouse[i].write(0xeb);  // give me data!
  mouse[i].read();      // ignore ack
  char mstat = mouse[i].read();
  char mx = mouse[i].read();
  char my = mouse[i].read();

  /* send the data to the Mac */
  Serial.print(":m");
  Serial.print((int)i);
  Serial.print("\t");
  Serial.print((int)mstat);
  Serial.print("\t");
  Serial.print((int)mx);
  Serial.print("\t");
  Serial.print((int)my);
  Serial.println();
  delay(20);
}
