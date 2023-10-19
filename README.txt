This is a text-based version of my latest project. It's where I will develop it before I add a graphical interface in Unity later.

The current features:
- A fully functional console-based "typing" effect
- Basic concepts for tracking player data through the game (e.g health, relationships, etc.)
- Conceptual demonstration that integrates the the basics of the looping concept (explained here below).
- Some writing on the introduction.

The Looping Concept:

Regardless of future story developments, which I have not decided on yet, the game will be based on the concept of loops both in the story and code.
What is a loop, you ask? Is this like that movie I watched one time with Bruce Willis? Both are very good questions, and I can provide a definitive "no" for the second. 

In the code, loops function as an innovative way to keep track of game events and ensure that every player choice factors into the eventual story outcome. Whereas many games function on decision-tree based logic for providing players with choices,
Looper creates a seperate instance of the common Loop type for every decision a player makes. After a inital prompt, which is the only hard-written instantiation of a loop, the player is free to create whatever loops they want based on their choices.
There are a few rules to this that keep the game moving forward based on a common story concept. First, a loop must always end under some conditions. These conditions depend on the loop, and include (but aren't limited to), the player reaching a certian
health value, the breaking or establishment of a relationship, or successfully accessing a certian location. Second, when a loop ends, it must either terminate that section of the story, or start a new loop to continue it. A loop may not simply end without
causing some sort of story event. Third, a loop must be either neglectable or unneglectable. A neglectable loop is one where the player can delay making a decision on, often leading to consequences later on depending on how many times the loop is put off.
If it isn't neglectable, a loop will force the player to make a decision when activated.

Loops get their content from the Script Manager, which sources dialouge options from an assets folder. At the end of the day, this is still in essence a decision tree, but it allows for greater flexibility and more specificity of player actions. 

All of this is a little confusing. Here's the visual flow for a basic loop if it helps.

How a loop is created: Action ---> new loop creation ---> player plays through loop content (as described in the script manager) ---> player chooses to end or neglect loop ---> Either new loop creation or execution of neglect conditions
If a loop is neglected: player neglects to end loop ---> neglect scripted content executes ---> neglected loop is stored in system memory until conditions for re-prompting are met ---> player is once again prompted to make a decision
If a loop is fully closed: closing content executes from Script Manager ---> control is returned to the originating loop

The player may also make choices during loop content to create another loop, which takes control, rendering the original loop on standby until it is completed or neglected.

In any case, when loops transfer control (either through closing, neglect, or actions within content), control is transferred to the loop that created the one transferring control, unless otherwise specified.

How this works in the game:
The player plays through the game as if it's a normal "your choices matter" RPG, and the concept of metaphysical time loops, in which every choice a human being makes eventually repays itself through karma (good or bad), is integrated in the story,
and is central to it's themes.

Hopefully this clears some things up!
