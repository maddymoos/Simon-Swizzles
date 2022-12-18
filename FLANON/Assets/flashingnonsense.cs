using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using KModkit;
using System.Text.RegularExpressions;
using Rnd = UnityEngine.Random;

public class flashingnonsense : MonoBehaviour
{

    public KMAudio Audio;
    public KMBombModule Module;
    public KMBombInfo Bomb;
    public Light[] why;
    public Color[] color;
    public Renderer[] renderers;
    public KMSelectable[] Buttons;
    public Material ghome;
    private static string[] GATES = { "0000", "0001", "0010", "0011", "0100", "0101", "0110", "0111", "1000", "1001", "1010", "1011", "1100", "1101", "1110", "1111" };
    private static string[] NAMES = { "OFF", "AND", "!LIMP", "LEFT", "!RIMP", "RIGHT", "XOR", "OR", "NOR", "XNOR", "!RIGHT", "RIMP", "!LEFT", "LIMP", "NAND", "ON" };
    private static string[] Phrases = { "Have fun solving this one!", "Hope you do well!", "This may be tough, but you can do it!", "Oof, this one looks tricky. Good luck.", "Not much to say here, but good luck!", "Uh, is this thing on?", "I turned myself into a logfile morty! I'm logfile Luna!", "Ok, this one is just impossible.", "You know the rules, and so do I.", "I think it might be *that one*... Yeah, that one!", "What's a colored squares?", "Cmon! Go for it!", "Nice log ya got there :P", "Alright, it's showtime!", "You can do it!", "Good to see ya using me!", "hey look i get to be here for a bit", "Hiyo!", "Oh god, not this again...", "Never let up!", "I'll always be here, rooting for you!", "Ah, I've been summoned.", "Looks like we've got a good one coming up!", "You offer to the shrine, and receive nothing.", ":)", "[ENGAGING SYSTEMS]", "[ENGAGING SYSTEMMIES]", "Hi, I'm temmie, and this is my friend, temmie!", "........Oh, wait, I need to say something witty here...", "Talking Sleeping Waving Waiting", "<Insert positive affirmation here>", "Oops sense machine broke guess we've lost all sense", "I see you've been LED on to these LEDs", "KappaPride", "Hey all, Scott here.", "Hmm... Perhaps... Knight to A5?", "So many colors!!!", "NOT NOT more like YES gate :P", "Hey, I believe in you!", "The only way to climb a mountain is to climb it. (What am I even saying anymore?)", "THE END IS LOADING", "All of her coworkers were gone. What did it mean?", "*silence*", "Tiky", "had to h", "I dont care that it might not be pride month, just be pride!", "Hi yes you are valid.", "It says says!", "...I'm running out of things to say. Well... uh... hi.", "What if I plugged myself in here?", "Nobody reads these...", "a", "red sus", "Hey. You're cute.", "i will literally stab you if you votesolve me", "this message should not appear. if it does, it's a bug.", "This message will never appear in the logfile. Isn't that strange?" };
    private static string[] Rainbow = { "1546546246236231", "3154154654624623", "2315315415465462", "6231231531541546", "4623623123153154", "5462462362312315" };
    /*132645                             
    "1546546246236231"                   
    3154 2315 6231 4623 5462        
    1546 3154 2315 6231 4623
    5462 1546 3154 2315 6231
    4623 5462 1546 3154 2315
    */
    private int StartTime, Time, kiill, owo;
    private int[] Gatestore = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 0 };
    private string inside;

    private bool Solved = false;
    static private int _moduleIdCounter = 1;
    private int _moduleId;
    void Awake()
    {
        _moduleId = _moduleIdCounter++;
        for (int i = 0; i < Buttons.Length; i++)
        {
            KMSelectable btn = Buttons[i];
            btn.OnInteract += delegate
            {
                HandlePress(btn);
                return false;
            };
        }
    }

    void HandlePress(KMSelectable btn)
    {
        if (!Solved)
        {
            int X = Array.IndexOf(Buttons, btn);
            Buttons[X].AddInteractionPunch();
            string superinside = Convert.ToInt32(inside, 2).ToString();
            if (X == owo && superinside == (Time % 60).ToString())
            {
                Module.HandlePass();
                Solved = true;
                StartCoroutine(solveddd());
                Debug.LogFormat("[Simon Swizzles #{0}]: Correct button pressed, at the right time. Module solved.", _moduleId);
                Audio.PlaySoundAtTransform("solve", Buttons[0].transform);
            }
            else if (X == owo && superinside != (Time % 60).ToString())
            {
                Module.HandleStrike();
                Debug.LogFormat("[Simon Swizzles #{0}]: Not quite. Correct button, wrong time.", _moduleId);
                Audio.PlaySoundAtTransform("strike", Buttons[0].transform);
            }
            else if (X != owo && superinside == (Time % 60).ToString())
            {
                Module.HandleStrike();
                Debug.LogFormat("[Simon Swizzles #{0}]: Not quite. Right time, wrong button.", _moduleId);
                Audio.PlaySoundAtTransform("strike", Buttons[0].transform);
            }
            else
            {
                Module.HandleStrike();
                Debug.LogFormat("[Simon Swizzles #{0}]: Oof, that's all wrong.", _moduleId);
                Audio.PlaySoundAtTransform("strike", Buttons[0].transform);
            }
        }
    }
    // Use this for initialization
    void Start()
    {
        StartTime = (int)Bomb.GetTime();
        float scalar = transform.lossyScale.x;
        foreach (Light l in why)
        {
            l.range *= scalar;
        }
        Gatestore = Gatestore.Shuffle().Shuffle().Shuffle();
        inside = Convert.ToString(Rnd.Range(1, 60), 2).PadLeft(6, '0');
        Debug.LogFormat("[Simon Swizzles #{0}]: Welcome to Simon Swizzles! {1} -Cooldoom5", _moduleId, Phrases[Rnd.Range(0, Phrases.Length)]);
        Debug.LogFormat("[Simon Swizzles #{0}]: Your internal binary string is {1}.", _moduleId, inside);
        Debug.LogFormat("[Simon Swizzles #{0}]: The gates, in reading order, are as follows: {1}.", _moduleId, Gatestore.Select(val => NAMES[val]).Join(", "));
        owo = Array.IndexOf(Gatestore, 0);
        owo = (Array.IndexOf(Gatestore, 15) + owo) % 16;
        Debug.LogFormat("[Simon Swizzles #{0}]: You must press the {1}{2} button, at X:{3}.", _moduleId, owo + 1, owo == 0 ? "st" : owo == 1 ? "nd" : owo == 2 ? "rd" : "th", Convert.ToInt32(inside, 2).ToString().PadLeft(2, '0'));
    }

    // Update is called once per frame
    void Update()
    {
        if (Time != (int)Bomb.GetTime() && !Solved)
        {
            Time = (int)Bomb.GetTime();
            if (StartTime == Time)
            {
                Solved = true;
                for (int i = 0; i < 16; i++)
                {
                    StartCoroutine(fader2(color[0], color[7], i));
                }
            }
            else
            {
                for (int i = 0; i < 16; i++)
                {
                    if (Rnd.Range(0, 10) != 0)
                    {
                        kiill = Rnd.Range(0, 6);
                        if (GATES[Gatestore[i]][int.Parse(Convert.ToString((int)Time % 60, 2).PadLeft(6, '0')[kiill].ToString()) * 2 + int.Parse(inside[kiill].ToString())] == '1')
                        {
                            StartCoroutine(fader(renderers[i].material.color, color[kiill + 1], i));
                        }
                        else
                        {
                            StartCoroutine(fader(renderers[i].material.color, color[0], i));
                        }
                    }
                    else
                    {
                        if (GATES[Gatestore[i]][int.Parse(Convert.ToString((int)Time % 60, 2).PadLeft(6, '0')[kiill].ToString()) * 2 + 1] == '1')
                        {
                            StartCoroutine(fader(renderers[i].material.color, color[7], i));
                        }
                        else
                        {
                            StartCoroutine(fader(renderers[i].material.color, color[0], i));
                        }
                    }
                }
            }
        }
    }
    IEnumerator solveddd()
    {
        yield return null;
        while (true)
        {
            for (int k = 0; k < 6; k++)
            {
                for (int i = 0; i < 16; i++)
                {
                    StartCoroutine(slowfader(renderers[i].material.color, color[Rainbow[k][i] - '0'], i));
                }
                yield return new WaitForSeconds(1.01f);
            }
        }
    }

    IEnumerator fader(Color basecol, Color end, int index)
    {
        yield return null;
        float time = 0;
        while (time < 1)
        {
            renderers[index].material.color = Color.Lerp(basecol, end, time);
            why[index].color = Color.Lerp(basecol, end, time);
            yield return null;
            time += UnityEngine.Time.deltaTime / .25f;
        }
        renderers[index].material.color = end;
        why[index].color = end;

    }
    IEnumerator fader2(Color basecol, Color end, int index)
    {
        yield return new WaitForSeconds(.1f);
        if(index == 0)
        {
            Audio.PlaySoundAtTransform("alarm", Buttons[0].transform);
        }
        yield return null;
        float time = 0;
        for (int i = 0; i < 4; i++)
        {
            time = 0;
            while (time < 1)
            {
                renderers[index].material.color = Color.Lerp(basecol, end, time);
                why[index].color = Color.Lerp(basecol, end, time);
                yield return null;
                time += UnityEngine.Time.deltaTime / .15f;
            }
            renderers[index].material.color = end; why[index].color = end;
            yield return new WaitForSeconds(.3f);
            time = 0;
            while (time < 1)
            {
                renderers[index].material.color = Color.Lerp(end, basecol, time);
                why[index].color = Color.Lerp(end, basecol, time);
                yield return null;
                time += UnityEngine.Time.deltaTime / .15f;
            }
            renderers[index].material.color = basecol; why[index].color = basecol;
            yield return new WaitForSeconds(.3f);
        }
        yield return new WaitForSeconds(.75f);
        Solved = false;
    }
    IEnumerator slowfader(Color basecol, Color end, int index)
    {
        yield return null;
        float time = 0;
        while (time < 1)
        {
            renderers[index].material.color = Color.Lerp(basecol, end, time);
            why[index].color = Color.Lerp(basecol, end, time);
            yield return null;
            time += UnityEngine.Time.deltaTime / 1f;
        }
        renderers[index].material.color = end;
        why[index].color = end;

    }

    //twitch plays
    #pragma warning disable 414
    private readonly string TwitchHelpMessage = @"To press a button (in reading order) at a certain time, use !{0} press [1-16] at [00-59]";
    #pragma warning restore 414

    IEnumerator ProcessTwitchCommand(string command)
    {
        string[] parameters = command.Split(' ');

        if (Regex.IsMatch(parameters[0], @"^\s*press\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;

            if (parameters.Length != 4)
            {
                yield return "sendtochaterror Parameter length invalid. Command ignored.";
                yield break;
            }

            if (!Regex.IsMatch(parameters[2], @"^\s*at\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
            {
                yield return "sendtochaterror The 3rd parameter is not \"at\". Command ignored.";
                yield break;
            }

            int Out;
            if (!int.TryParse(parameters[1], out Out))
            {
                yield return "sendtochaterror The button number given is not valid. Command ignored.";
                yield break;
            }

            if (Out < 1 || Out > 16)
            {
                yield return "sendtochaterror The button number given is not 1-16. Command ignored.";
                yield break;
            }

            int Out2;
            if (!int.TryParse(parameters[3], out Out2))
            {
                yield return "sendtochaterror The timer number given is not valid. Command ignored.";
                yield break;
            }

            if (Out2 < 0 || Out2 > 59)
            {
                yield return "sendtochaterror The timer number given is not 00-59. Command ignored.";
                yield break;
            }

            if (parameters[3].Length != 2)
            {
                yield return "sendtochaterror The timer length must be 2. Command ignored.";
                yield break;
            }

            while (((int)Bomb.GetTime()) % 60 != Out2)
            {
                yield return "trycancel The command is cancelled due to a cancel request.";
            }

            Buttons[Out - 1].OnInteract();
        }
    }

    IEnumerator TwitchHandleForcedSolve()
    {
        while (Solved || Convert.ToInt32(inside, 2).ToString() != (Time % 60).ToString()) yield return true;
        Buttons[owo].OnInteract();
    }
}
