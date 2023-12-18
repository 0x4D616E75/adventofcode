original source: [https://adventofcode.com/2023/day/15](https://adventofcode.com/2023/day/15)
## --- Day 15: Lens Library ---
The newly-focused parabolic reflector dish is sending all of the collected light to a point on the side of yet another mountain - the largest mountain on Lava Island. As you approach the mountain, you find that the light is being collected by the wall of a large facility embedded in the mountainside.

You find a door under a large sign that says "Lava Production Facility" and next to a smaller sign that says "Danger - Personal Protective Equipment required beyond this point".

As you step inside, you are immediately greeted by a somewhat panicked reindeer wearing goggles and a loose-fitting [hard hat](https://en.wikipedia.org/wiki/Hard_hat). The reindeer leads you to a shelf of goggles and hard hats (you quickly find some that fit) and then further into the facility. At one point, you pass a button with a faint snout mark and the label "PUSH FOR HELP". No wonder you were loaded into that [trebuchet](1) so quickly!

You pass through a final set of doors surrounded with even more warning signs and into what must be the room that collects all of the light from outside. As you admire the large assortment of lenses available to further focus the light, the reindeer brings you a book titled "Initialization Manual".

"Hello!", the book cheerfully begins, apparently unaware of the concerned reindeer reading over your shoulder. "This procedure will let you bring the Lava Production Facility online - all without burning or melting anything unintended!"

"Before you begin, please be prepared to use the Holiday ASCII String Helper algorithm (appendix 1A)." You turn to appendix 1A. The reindeer leans closer with interest.

The HASH algorithm is a way to turn any [string](https://en.wikipedia.org/wiki/String_(computer_science)) of characters into a single <em>number</em> in the range 0 to 255. To run the HASH algorithm on a string, start with a <em>current value</em> of <code>0</code>. Then, for each character in the string starting from the beginning:


 - Determine the [ASCII code](https://en.wikipedia.org/wiki/ASCII#Printable_characters) for the current character of the string.
 - Increase the <em>current value</em> by the ASCII code you just determined.
 - Set the <em>current value</em> to itself multiplied by <code>17</code>.
 - Set the <em>current value</em> to the [remainder](https://en.wikipedia.org/wiki/Modulo) of dividing itself by <code>256</code>.

After following these steps for each character in the string in order, the <em>current value</em> is the output of the HASH algorithm.

So, to find the result of running the HASH algorithm on the string <code>HASH</code>:


 - The <em>current value</em> starts at <code>0</code>.
 - The first character is <code>H</code>; its ASCII code is <code>72</code>.
 - The <em>current value</em> increases to <code>72</code>.
 - The <em>current value</em> is multiplied by <code>17</code> to become <code>1224</code>.
 - The <em>current value</em> becomes <code><em>200</em></code> (the remainder of <code>1224</code> divided by <code>256</code>).
 - The next character is <code>A</code>; its ASCII code is <code>65</code>.
 - The <em>current value</em> increases to <code>265</code>.
 - The <em>current value</em> is multiplied by <code>17</code> to become <code>4505</code>.
 - The <em>current value</em> becomes <code><em>153</em></code> (the remainder of <code>4505</code> divided by <code>256</code>).
 - The next character is <code>S</code>; its ASCII code is <code>83</code>.
 - The <em>current value</em> increases to <code>236</code>.
 - The <em>current value</em> is multiplied by <code>17</code> to become <code>4012</code>.
 - The <em>current value</em> becomes <code><em>172</em></code> (the remainder of <code>4012</code> divided by <code>256</code>).
 - The next character is <code>H</code>; its ASCII code is <code>72</code>.
 - The <em>current value</em> increases to <code>244</code>.
 - The <em>current value</em> is multiplied by <code>17</code> to become <code>4148</code>.
 - The <em>current value</em> becomes <code><em>52</em></code> (the remainder of <code>4148</code> divided by <code>256</code>).

So, the result of running the HASH algorithm on the string <code>HASH</code> is <code><em>52</em></code>.

The <em>initialization sequence</em> (your puzzle input) is a comma-separated list of steps to start the Lava Production Facility. <em>Ignore newline characters</em> when parsing the initialization sequence. To verify that your HASH algorithm is working, the book offers the sum of the result of running the HASH algorithm on each step in the initialization sequence.

For example:

<pre>
<code>rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7</code>
</pre>

This initialization sequence specifies 11 individual steps; the result of running the HASH algorithm on each of the steps is as follows:


 - <code>rn=1</code> becomes <code><em>30</em></code>.
 - <code>cm-</code> becomes <code><em>253</em></code>.
 - <code>qp=3</code> becomes <code><em>97</em></code>.
 - <code>cm=2</code> becomes <code><em>47</em></code>.
 - <code>qp-</code> becomes <code><em>14</em></code>.
 - <code>pc=4</code> becomes <code><em>180</em></code>.
 - <code>ot=9</code> becomes <code><em>9</em></code>.
 - <code>ab=5</code> becomes <code><em>197</em></code>.
 - <code>pc-</code> becomes <code><em>48</em></code>.
 - <code>pc=6</code> becomes <code><em>214</em></code>.
 - <code>ot=7</code> becomes <code><em>231</em></code>.

In this example, the sum of these results is <code><em>1320</em></code>. Unfortunately, the reindeer has stolen the page containing the expected verification number and is currently running around the facility with it excitedly.

Run the HASH algorithm on each step in the initialization sequence. <em>What is the sum of the results?</em> (The initialization sequence is one long line; be careful when copy-pasting it.)


