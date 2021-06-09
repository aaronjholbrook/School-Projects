#Setup
import pyeda
import graphviz
#import os
from pyeda.inter import *
#os.environ["PATH"] += os.pathsep + 'C:/Program Files/Graphviz 2.44.1/bin'
x = bddvars('x', 5)
y = bddvars('y', 5)
z = bddvars('z', 5)
allEdges = list()
evenEdges = list()
primeEdges = list()

#4.1 methods
def isPrime(n):
    if n == 0:
        return False

    if n > 1:
        for i in range(2, n):
            if n % i == 0:
                return False
    return True

#Where k is the k in 2^k nodes, check for edge connections on
#(a + 3)%32 = b%32 or (a + 8)%32 = b%32. When meeting a connect
#condition convert node numbers to binary strings and concatenate.
#Then append to proper edge lists: all, prime nodes, even nodes.
def createEdges(k:int):
    for a in range (0, (2**k)):
        for b in range (0, (2**k)):
            if (a + 3) % (2**k) == (b%(2**k)):
                current = format(a, '05b') + format(b, '05b')
                allEdges.append(current)
                if (a % 2 == 0) & (b % 2 == 0):
                    evenEdges.append(current)
                if (isPrime(a) & (a != 2)) & (isPrime(b) & (b != 2)):
                    primeEdges.append(current)
            if (a + 8) % (2**k) == (b%(2**k)):
                current = format(a, '05b') + format(b, '05b')
                allEdges.append(current)
                if (a % 2 == 0) & (b % 2 == 0):
                    evenEdges.append(current)
                if (isPrime(a) & (a != 2)) & (isPrime(b) & (b != 2)):
                    primeEdges.append(current)

#Given a list of edges (assuming they're in the 10 digit binary node pair
# format) build the expression for those edges.
def buildExpression(input: list()):
    currentExpression = list()
    #currentExpression.append("Or(") #All edge expressions are or'd together.

    for string in input:
        #currentExpression.append("And(") #Edge variables are all and'd
        currentExpression.append("(")
        for i in range(10):
            #x variables
            if (i <= 4):
                if string[i] == '0':
                    #currentExpression.append("~x[" + str(i) + "], ")
                    currentExpression.append("~x[" + str(i) + "] & ")
                else:
                    #currentExpression.append("x[" + str(i) + "], ")
                    currentExpression.append("x[" + str(i) + "] & ")
            #y variables
            elif (i <= 8):
                if string[i] == '0':
                    #currentExpression.append("~y[" + str((i - 5)) + "], ")
                    currentExpression.append("~y[" + str((i - 5)) + "] & ")
                else:
                    #currentExpression.append("y[" + str((i - 5)) + "], ")
                    currentExpression.append("y[" + str((i - 5)) + "] & ")
            #Final digit, enclosing the And of the expression
            elif (i == 9):
                if string[i] == '0':
                    currentExpression.append("~y[" + str((i - 5)) + "])")
                    #currentExpression.append(", ")
                    currentExpression.append(" | ")
                else:
                    currentExpression.append("y[" + str((i - 5)) + "])")
                    #currentExpression.append(", ")
                    currentExpression.append(" | ")

    #Remove extra ", " and replace with ")"
    currentExpression.pop()
    #currentExpression.append(")")

    return expr2bdd(expr("".join(currentExpression), simplify=False))

#4.2 methods
#Take a BDD and compose it with itself (input∘input)
def composeSelf(input:BinaryDecisionDiagram):
    R1 = input.compose({y[0]: z[0], y[1]: z[1], y[2]: z[2],
                                            y[3]: z[3], y[4]: z[4]})
    R2 = input.compose({x[0]: z[0], x[1]: z[1], x[2]: z[2],
                                            x[3]: z[3], x[4]: z[4]})
    return (R1 & R2).smoothing(z)

#Compose a BDD with another BDD (inputA∘inputB)
def compose(inputA:BinaryDecisionDiagram, inputB:BinaryDecisionDiagram):
    R1 = inputA.compose({y[0]: z[0], y[1]: z[1], y[2]: z[2],
                                            y[3]: z[3], y[4]: z[4]})
    R2 = inputB.compose({x[0]: z[0], x[1]: z[1], x[2]: z[2],
                                            x[3]: z[3], x[4]: z[4]})
    return (R1 & R2).smoothing(z)

#4.3 method
def bddStar(input:BinaryDecisionDiagram):
    H = input
    HPrime = -1
    count = 0
    while (H != HPrime):
        HPrime = H
        H = (HPrime) | (compose(HPrime, RR))
        #H = expr2bdd(bdd2expr(H).simplify())
        count += 1
    print("bddStar took: " + str(count) + " iterations.")
    return H

#4.1: RR, EVEN, PRIME (See attached excel files for by hand work.)
createEdges(5) #Fill edge lists for 32 node graph

#RR, the set of all edges (the whole graph).
RR = buildExpression(allEdges)
print("See RR.gv.pdf\n")

#EVEN: The nodes with even number valuings (0, 2, 4, 6, ... 30)
EVEN = buildExpression(evenEdges)
print("See EVEN.gv.pdf\n")

#PRIME: The nodes with prime number valuings 
#(2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31)
PRIME = buildExpression(primeEdges)
print("See PRIME.gv.pdf\n")

#4.2: RR2 aka R compose R which is all node pairs that can reach each other
#in two or less steps. [∃𝑧1, … , 𝑧k: 𝑅(𝑥1, 𝑥2, … , 𝑥𝑘; 𝑧1, 𝑧2, … , 𝑧𝑘) ∧ 
#(𝑧1, 𝑧2, … , 𝑧𝑘; 𝑦1, 𝑦2, … , 𝑦𝑘)]
RR2 = composeSelf(RR)
print("See RR2.gv.pdf\n")

#4.3: RR2star all node pairs where one reaches the other in 2 steps
RR2star = bddStar(RR2)
print("RR2star: " + RR2star.__str__() + "\n")

#4.4: PE all node pairs (u, v) where u is prime and v is even and u reaches
#v in an even number of steps.
#And(PRIME, EVEN, RR2star)or PRIME & EVEN & RR2star
PE = And(PRIME, EVEN, RR2star)
print("PE: " + PE.__str__())

#Print BDDs to PDF (Requires graphviz)
graphviz.Source(RR.to_dot()).render('RR.gv', view=False)
graphviz.Source(EVEN.to_dot()).render('EVEN.gv', view=False)
graphviz.Source(PRIME.to_dot()).render('PRIME.gv', view=False)
graphviz.Source(RR2.to_dot()).render('RR2.gv', view=False)
graphviz.Source(RR2star.to_dot()).render('RR2star.gv', view=False)
graphviz.Source(PE.to_dot()).render('PE.gv', view=False)