# Sentinels - Genetic Algorithm Prototype

Sentinels is my final project for the Computer Science BSc in the University of
London.

I am not proficient in C# nor Unity3D and the only experience I had with any of
those was in the Game Development module during the Course. If you are looking
into this repository with the goal of learning any of those technologies,
please be advised that this is most probably not your better source.

This repository contains the first feature prototype of the project.  This
prototype was chosen because I felt it is probably the most difficult in the
project and as per instructed, that is where I should start.

## Goals

The goals of this prototype are as follows:

- Learn how to use Git and Unity3D together
- Learn and demonstrate how to write unit tests for Unity3D
- Learn and demonstrate how to implement a Genetic Algorithm in C#
- Learn and demonstrate how to change GameObjects based on the Genetic
  Algorithm

### Version control

Unity3D generates several files that may not need to be versioned, and the game
uses lots of third party packages that are versioned elsewhere. Understanding
the best practices to version files from Unity3D is a first minor goal of this
prototype.

### Unit tests

Unit tests are usually done with a set of tools that can be overwhelming for
those who are new to a particular programming language. The programmer not only
have to cope with the challenges of building a solution, but also with the
language syntax, best practices and tools that can be significantly different
from other previously known languages.

An exploratory quick research showed that unit tests are sometimes neglected
when building games with Unity3D and that these unit tests have the additional
challenge of having to integrate well with Unity3D game objects, Unity Editor
Interface and Unity Play environment.

Given these challenges, the first goal of this prototype is to implement unit
tests within Unity3D and describe the process.


### Genetic Algorithm

One of the game's features is to use a Genetic Algorithm to breed enemies while
the game is in play with the goal that the player feels to be playing against
an Artificial Intelligence. Creating an AI that is really capable of evolving
incrementally better enemies is beyond the scope of this game, but the
perception that you are fighting an advanced AI is important to the game theme.
While the enemy ships will be simply given additional power over the
generations to make them harder to play against, it is crucial that the
evolution should be visually and comportamentally perceived.

The implementation of the algorithm will closely follow the implementation
presented in the Artificial Intelligence module by professor Yee-King. That
algorithm was written in Python and the first step for this prototype is to
translate it into C#. This should give me the opportunity to better understand
C# and the tools needed to work with this language.

### Evolving spaceships

A final challenge in this implementation is to make the algorithm control both
the appearence and the behaviour of the space ship. To achieve this the
prototype will need to integrate the Unity3D game object, Editor interface and
the genetic algorithm.

## Testing 

To create a first test open Window->General->Test Runner and use the provided
buttons to create the test folders and test files.
