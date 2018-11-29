using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class QuestionManager : ISingletonHandler
{
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

    public enum Categorie
    {
        Sports,
        Science,
        History,
        Geography,
        Music,
        Math,
    }

    private List<QuestionItem> questions;
    private List<Guid> alreadyAnswered;

    public void Init()
    {
        questions = new List<QuestionItem>
        {
            new QuestionItem
            {
                Question = "What is a positive integer that is equal to the sum of its proper positive divisors (excluding the number itself) called?",
                Answer = new List<Tuple<string, bool>>
                {
                    new Tuple<string, bool>("A perfect number", true),
                    new Tuple<string, bool>("A good number", false),
                    new Tuple<string, bool>("A natural number", false)
                },
                Categorie = Categorie.Math,
                Difficulty = Difficulty.Easy,
            },
            new QuestionItem
            {
                Question = "Is 8128 a perfect number?",
                Answer = new List<Tuple<string, bool>>
                {
                    new Tuple<string, bool>("No", false),
                    new Tuple<string, bool>("Yes, it is", true),
                    new Tuple<string, bool>("", false)
                },
                Categorie = Categorie.Math,
                Difficulty = Difficulty.Easy,
            },
            new QuestionItem
            {
                Question = "Choose the perfect number from the answers?",
                Answer = new List<Tuple<string, bool>>
                {
                    new Tuple<string, bool>("46", false),
                    new Tuple<string, bool>("32", false),
                    new Tuple<string, bool>("28", true)
                },
                Categorie = Categorie.Math,
                Difficulty = Difficulty.Easy,
            },
            new QuestionItem
            {
                Question = "How much is 45 multiplied by 95?",
                Answer = new List<Tuple<string, bool>>
                {
                    new Tuple<string, bool>("4275", true),
                    new Tuple<string, bool>("4050", false),
                    new Tuple<string, bool>("4525", false)
                },
                Categorie = Categorie.Math,
                Difficulty = Difficulty.Easy,
            },
            new QuestionItem
            {
                Question = "What is not a prime number",
                Answer = new List<Tuple<string, bool>>
                {
                    new Tuple<string, bool>("47", false),
                    new Tuple<string, bool>("67", false),
                    new Tuple<string, bool>("87", true)
                },
                Categorie = Categorie.Math,
                Difficulty = Difficulty.Easy,
            },
            new QuestionItem
            {
                Question = "What is the sum of the angles of a triangle?",
                Answer = new List<Tuple<string, bool>>
                {
                    new Tuple<string, bool>("145 degrees", false),
                    new Tuple<string, bool>("180 degrees", true),
                    new Tuple<string, bool>("270 degrees", false)
                },
                Categorie = Categorie.Math,
                Difficulty = Difficulty.Easy,
            },
            new QuestionItem
            {
                Question = "This musician is often referred to as the fifth Beatle.",
                Answer = new List<Tuple<string, bool>>
                {
                    new Tuple<string, bool>("Pete Best", true),
                    new Tuple<string, bool>("Peter Star", false),
                    new Tuple<string, bool>("Ringo Starr", false)
                },
                Categorie = Categorie.Music,
                Difficulty = Difficulty.Easy,
            },
            new QuestionItem
            {
                Question = "What is the name of Nirvana's debut album?",
                Answer = new List<Tuple<string, bool>>
                {
                    new Tuple<string, bool>("Nevermind", false),
                    new Tuple<string, bool>("In Utero", false),
                    new Tuple<string, bool>("Bleach", true)
                },
                Categorie = Categorie.Music,
                Difficulty = Difficulty.Medium,
            },
            new QuestionItem
            {
                Question = "Who is the lead singer of Pearl Jam?",
                Answer = new List<Tuple<string, bool>>
                {
                    new Tuple<string, bool>("Eddie Vedder", true),
                    new Tuple<string, bool>("Mike McCready", false),
                    new Tuple<string, bool>("Stone Gossard", false)
                },
                Categorie = Categorie.Music,
                Difficulty = Difficulty.Medium,
            },
            new QuestionItem
            {
                Question = "What musician won the Nobel Prize for Literature in 2016?",
                Answer = new List<Tuple<string, bool>>
                {
                    new Tuple<string, bool>("Eric Clapton", false),
                    new Tuple<string, bool>("Bob Dylan ", true),
                    new Tuple<string, bool>("Neil Young", false)
                },
                Categorie = Categorie.Music,
                Difficulty = Difficulty.Medium,
            },
            new QuestionItem
            {
                Question = "What famous female singer died of alcohol poisoning in 2011 at the age of 27?",
                Answer = new List<Tuple<string, bool>>
                {
                    new Tuple<string, bool>("Amy Winehouse", true),
                    new Tuple<string, bool>("Adele", false),
                    new Tuple<string, bool>("Lana Del Rey", false)
                },
                Categorie = Categorie.Music,
                Difficulty = Difficulty.Easy,
            },
            new QuestionItem
            {
                Question = "Who is the most successful UK solo artist in the US?",
                Answer = new List<Tuple<string, bool>>
                {
                    new Tuple<string, bool>("David Bowie", false),
                    new Tuple<string, bool>("John Lennon", false),
                    new Tuple<string, bool>("Elton John", true)
                },
                Categorie = Categorie.Music,
                Difficulty = Difficulty.Medium,
            },
            new QuestionItem
            {
                Question = "What male tennis player has won the most Grand Slam titles?",
                Answer = new List<Tuple<string, bool>>
                {
                    new Tuple<string, bool>("Roger Federer", true),
                    new Tuple<string, bool>("Andre Agassi", false),
                    new Tuple<string, bool>("Boris Becker", false)
                },
                Categorie = Categorie.Sports,
                Difficulty = Difficulty.Medium,
            },
            new QuestionItem
            {
                Question = "How many holes are there in a full round of golf?",
                Answer = new List<Tuple<string, bool>>
                {
                    new Tuple<string, bool>("16", false),
                    new Tuple<string, bool>("18", true),
                    new Tuple<string, bool>("21", false)
                },
                Categorie = Categorie.Sports,
                Difficulty = Difficulty.Easy,
            },
            new QuestionItem
            {
                Question = "What year was the first Super Bowl played?",
                Answer = new List<Tuple<string, bool>>
                {
                    new Tuple<string, bool>("1949", false),
                    new Tuple<string, bool>("1957", false),
                    new Tuple<string, bool>("1967", true)
                },
                Categorie = Categorie.Sports,
                Difficulty = Difficulty.Medium,
            },
            new QuestionItem
            {
                Question = "In what year was the first modern Olympic Games held?",
                Answer = new List<Tuple<string, bool>>
                {
                    new Tuple<string, bool>("1896", true),
                    new Tuple<string, bool>("1912", false),
                    new Tuple<string, bool>("1916", false)
                },
                Categorie = Categorie.Sports,
                Difficulty = Difficulty.Medium,
            },
            new QuestionItem
            {
                Question = "What NFL Quarterback has been in the most Super Bowls?",
                Answer = new List<Tuple<string, bool>>
                {
                    new Tuple<string, bool>("Peyton Manning", false),
                    new Tuple<string, bool>("Joe Montan", false),
                    new Tuple<string, bool>("Tom Brady", true)
                },
                Categorie = Categorie.Sports,
                Difficulty = Difficulty.Medium,
            },
            new QuestionItem
            {
                Question = "Brazil was eliminated in the 2014 world cup by what team?",
                Answer = new List<Tuple<string, bool>>
                {
                    new Tuple<string, bool>("Austria", false),
                    new Tuple<string, bool>("Argentina", false),
                    new Tuple<string, bool>("Germany", true)
                },
                Categorie = Categorie.Sports,
                Difficulty = Difficulty.Easy,
            },
            new QuestionItem
            {
                Question = "Which is the only country to have played in every World Cup?",
                Answer = new List<Tuple<string, bool>>
                {
                    new Tuple<string, bool>("Brazil", true),
                    new Tuple<string, bool>("Germany", false),
                    new Tuple<string, bool>("Italy", false)
                },
                Categorie = Categorie.Sports,
                Difficulty = Difficulty.Easy,
            },
            new QuestionItem
            {
                Question = "How long is an eon in geology?",
                Answer = new List<Tuple<string, bool>>
                {
                    new Tuple<string, bool>("A million years", false),
                    new Tuple<string, bool>("A billion years ", true),
                    new Tuple<string, bool>("A trillion years", false)
                },
                Categorie = Categorie.Science,
                Difficulty = Difficulty.Medium,
            },
            new QuestionItem
            {
                Question = "How long does it take for light from the Sun to reach Earth?",
                Answer = new List<Tuple<string, bool>>
                {
                    new Tuple<string, bool>("1 minute and 30 seconds", false),
                    new Tuple<string, bool>("4 minutes and 10 seconds", false),
                    new Tuple<string, bool>("8 minutes and 20 seconds", true)
                },
                Categorie = Categorie.Science,
                Difficulty = Difficulty.Medium,
            },
            new QuestionItem
            {
                Question = "How many time zones are there in the world?",
                Answer = new List<Tuple<string, bool>>
                {
                    new Tuple<string, bool>("12", false),
                    new Tuple<string, bool>("16", false),
                    new Tuple<string, bool>("24", true)
                },
                Categorie = Categorie.Science,
                Difficulty = Difficulty.Medium,
            },
            new QuestionItem
            {
                Question = "Which planet has the most moons?",
                Answer = new List<Tuple<string, bool>>
                {
                    new Tuple<string, bool>("Jupiter", true),
                    new Tuple<string, bool>("Saturn", false),
                    new Tuple<string, bool>("Neptune", false)
                },
                Categorie = Categorie.Science,
                Difficulty = Difficulty.Medium,
            },
            new QuestionItem
            {
                Question = "What is the first element on the periodic table?",
                Answer = new List<Tuple<string, bool>>
                {
                    new Tuple<string, bool>("Hydrogen", true),
                    new Tuple<string, bool>("Lithium", false),
                    new Tuple<string, bool>("Carbon", false)
                },
                Categorie = Categorie.Science,
                Difficulty = Difficulty.Easy,
            },
            new QuestionItem
            {
                Question = "What blood type do you need to be a universal donor?",
                Answer = new List<Tuple<string, bool>>
                {
                    new Tuple<string, bool>("O-", true),
                    new Tuple<string, bool>("AB+", false),
                    new Tuple<string, bool>("AB-", false)
                },
                Categorie = Categorie.Science,
                Difficulty = Difficulty.Easy,
            },
            new QuestionItem
            {
                Question = "Diamonds are made up almost entirely of what element?",
                Answer = new List<Tuple<string, bool>>
                {
                    new Tuple<string, bool>("Calcium", false),
                    new Tuple<string, bool>("Cobalt", false),
                    new Tuple<string, bool>("Carbon", true)
                },
                Categorie = Categorie.Science,
                Difficulty = Difficulty.Easy,
            },
            new QuestionItem
            {
                Question = "Diamonds are made up almost entirely of what element?",
                Answer = new List<Tuple<string, bool>>
                {
                    new Tuple<string, bool>("Earth", false),
                    new Tuple<string, bool>("Mars", false),
                    new Tuple<string, bool>("Jupiter", true)
                },
                Categorie = Categorie.Science,
                Difficulty = Difficulty.Medium,
            },
            new QuestionItem
            {
                Question = "What is the fastest land snake in the world?",
                Answer = new List<Tuple<string, bool>>
                {
                    new Tuple<string, bool>("Black Mamba", true),
                    new Tuple<string, bool>("King cobra", false),
                    new Tuple<string, bool>("Common death adder", false)
                },
                Categorie = Categorie.Science,
                Difficulty = Difficulty.Hard,
            },
            new QuestionItem
            {
                Question = "On what day of the week did the Normandy Invasion of June 6, 1944 take place?",
                Answer = new List<Tuple<string, bool>>
                {
                    new Tuple<string, bool>("Tuesday", true),
                    new Tuple<string, bool>("Friday", false),
                    new Tuple<string, bool>("Saturday", false)
                },
                Categorie = Categorie.History,
                Difficulty = Difficulty.Medium,
            },
            new QuestionItem
            {
                Question = "What was the first capital city of the United States?",
                Answer = new List<Tuple<string, bool>>
                {
                    new Tuple<string, bool>("Chicago", false),
                    new Tuple<string, bool>("New York", false),
                    new Tuple<string, bool>("Philadelphia", true)
                },
                Categorie = Categorie.History,
                Difficulty = Difficulty.Hard,
            },
            new QuestionItem
            {
                Question = "Who painted the ceiling of the Sistine Chapel?",
                Answer = new List<Tuple<string, bool>>
                {
                    new Tuple<string, bool>("Vincent van Gogh", false),
                    new Tuple<string, bool>("Michelangelo", true),
                    new Tuple<string, bool>("Pablo Picasso", false)
                },
                Categorie = Categorie.History,
                Difficulty = Difficulty.Easy,
            },
            new QuestionItem
            {
                Question = "What is the world's smallest country?",
                Answer = new List<Tuple<string, bool>>
                {
                    new Tuple<string, bool>("Malta", false),
                    new Tuple<string, bool>("Monaco", false),
                    new Tuple<string, bool>("Vatican", true)
                },
                Categorie = Categorie.History,
                Difficulty = Difficulty.Medium,
            },
            new QuestionItem
            {
                Question = "What famous actor became Governor of California in 2003?",
                Answer = new List<Tuple<string, bool>>
                {
                    new Tuple<string, bool>("Arnold Schwarzenegger", true),
                    new Tuple<string, bool>("Sylvester Stallone", false),
                    new Tuple<string, bool>("Chuck Norris", false)
                },
                Categorie = Categorie.History,
                Difficulty = Difficulty.Easy,
            },
            new QuestionItem
            {
                Question = "What is the name for the Greek goddess of victory?",
                Answer = new List<Tuple<string, bool>>
                {
                    new Tuple<string, bool>("Hera", false),
                    new Tuple<string, bool>("Nike", true),
                    new Tuple<string, bool>("Selene", false)
                },
                Categorie = Categorie.History,
                Difficulty = Difficulty.Easy,
            },
            new QuestionItem
            {
                Question = "Which one of the seven ancient wonders of the world is still standing today?",
                Answer = new List<Tuple<string, bool>>
                {
                    new Tuple<string, bool>("The Great Pyramid of Giza", true),
                    new Tuple<string, bool>("Mausoleum at Halicarnassus", false),
                    new Tuple<string, bool>("Lighthouse of Alexandria", false)
                },
                Categorie = Categorie.History,
                Difficulty = Difficulty.Easy,
            },
            new QuestionItem
            {
                Question = "In what year was Queen Elizabeth II born?",
                Answer = new List<Tuple<string, bool>>
                {
                    new Tuple<string, bool>("1911", false),
                    new Tuple<string, bool>("1926", true),
                    new Tuple<string, bool>("1935", false)
                },
                Categorie = Categorie.History,
                Difficulty = Difficulty.Medium,
            },
            new QuestionItem
            {
                Question = "In what year did World War II end?",
                Answer = new List<Tuple<string, bool>>
                {
                    new Tuple<string, bool>("1944", false),
                    new Tuple<string, bool>("1945", true),
                    new Tuple<string, bool>("1946", false)
                },
                Categorie = Categorie.History,
                Difficulty = Difficulty.Easy,
            },
            new QuestionItem
            {
                Question = "Who was the first man to set foot on the moon?",
                Answer = new List<Tuple<string, bool>>
                {
                    new Tuple<string, bool>("Buzz Aldrin", false),
                    new Tuple<string, bool>("Michael Collins", false),
                    new Tuple<string, bool>("Neil Armstrong", true)
                },
                Categorie = Categorie.History,
                Difficulty = Difficulty.Easy,
            },
            new QuestionItem
            {
                Question = "What city was buried by Mount Vesuvius in 79 A.D.?",
                Answer = new List<Tuple<string, bool>>
                {
                    new Tuple<string, bool>("Angkor", false),
                    new Tuple<string, bool>("Palmyra", false),
                    new Tuple<string, bool>("Pompeii", true)
                },
                Categorie = Categorie.History,
                Difficulty = Difficulty.Hard,
            },
            new QuestionItem
            {
                Question = "The atomic bomb was dropped on Hiroshima in which decade?",
                Answer = new List<Tuple<string, bool>>
                {
                    new Tuple<string, bool>("1940's", true),
                    new Tuple<string, bool>("1950's", false),
                    new Tuple<string, bool>("1960's", false)
                },
                Categorie = Categorie.History,
                Difficulty = Difficulty.Easy,
            },
            new QuestionItem
            {
                Question = "With over 35 million residents, what is the most populous city in the world?",
                Answer = new List<Tuple<string, bool>>
                {
                    new Tuple<string, bool>("Mumbai", false),
                    new Tuple<string, bool>("Mexico City", false),
                    new Tuple<string, bool>("Tokyo", true)
                },
                Categorie = Categorie.Geography,
                Difficulty = Difficulty.Easy,
            },
            new QuestionItem
            {
                Question = "Which country (and it's territories) cover the most time zones?",
                Answer = new List<Tuple<string, bool>>
                {
                    new Tuple<string, bool>("Antarctica", false),
                    new Tuple<string, bool>("France", true),
                    new Tuple<string, bool>("United Kingdom", false)
                },
                Categorie = Categorie.Geography,
                Difficulty = Difficulty.Medium,
            },
            new QuestionItem
            {
                Question = "What is the longest river in the world?",
                Answer = new List<Tuple<string, bool>>
                {
                    new Tuple<string, bool>("Amazon", true),
                    new Tuple<string, bool>("Nile", false),
                    new Tuple<string, bool>("Rio Grande", false)
                },
                Categorie = Categorie.Geography,
                Difficulty = Difficulty.Easy,
            },
            new QuestionItem
            {
                Question = "What country has the largest land mass?",
                Answer = new List<Tuple<string, bool>>
                {
                    new Tuple<string, bool>("Australia", false),
                    new Tuple<string, bool>("Russia", true),
                    new Tuple<string, bool>("United States of America", false),
                },
                Categorie = Categorie.Geography,
                Difficulty = Difficulty.Easy,
            },
            new QuestionItem
            {
                Question = "What is the largest island in the Caribbean Sea?",
                Answer = new List<Tuple<string, bool>>
                {
                    new Tuple<string, bool>("Cuba", true),
                    new Tuple<string, bool>("Grand Bahama Island", false),
                    new Tuple<string, bool>("Puerto Rico", false)
                },
                Categorie = Categorie.Geography,
                Difficulty = Difficulty.Medium,
            },
            new QuestionItem
            {
                Question = "What is earth's largest ocean by surface size?",
                Answer = new List<Tuple<string, bool>>
                {
                    new Tuple<string, bool>("Atlantic ", false),
                    new Tuple<string, bool>("Indian", false),
                    new Tuple<string, bool>("Pacific", true)
                },
                Categorie = Categorie.Geography,
                Difficulty = Difficulty.Easy,
            }
        };

        Godot.GD.Print("Questions loaded:" + questions.Count);

        alreadyAnswered = new List<Guid>();
    }

    public QuestionItem Question()
    {
        QuestionItem current = null;
        var q = questions.Where(x => !alreadyAnswered.Contains(x.Id));
        if (q.Count() == 0)
        {
            // reset the already answered questions
            alreadyAnswered.Clear();
            q = questions.Where(x => !alreadyAnswered.Contains(x.Id));
        }
        if (q.Count() == 1)
        {
            current = q.First();

        }
        else
        {
            current = q.ElementAt(SharedFunctions.Instance.RandRand(0, q.Count()));
        }
        return current;
    }

    public void AnsweredQuestion(QuestionItem question)
    {
        alreadyAnswered.Add(question.Id);
    }

    public bool AlreadyAnswered(Guid id)
    {
        return alreadyAnswered.Any(x => x == id);
    }
}
