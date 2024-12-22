using System;
using System.Linq;
using System.Collections.Generic;
using modoff.Model;
using modoff.Util;

namespace modoff.Services
{
    public class ProfileService
    {
        private readonly DBContext ctx;
        private List<ProfileAnswer> allAnswersFromData;
        private List<ProfileQuestion> allQuestionsFromData;

        public ProfileService(DBContext ctx)
        {
            this.ctx = ctx;

            ProfileQuestionData questionData = XmlUtil.DeserializeXml<ProfileQuestionData>(XmlUtil.ReadResourceXmlString("questiondata"));

            allAnswersFromData = new List<ProfileAnswer>();
            allQuestionsFromData = new List<ProfileQuestion>();

            foreach(var list in questionData.Lists)
            {
                foreach(var question in list.Questions)
                {
                    allQuestionsFromData.Add(question);
                    foreach(var answer in question.Answers)
                    {
                        allAnswersFromData.Add(answer);
                    }
                }
            }
        }

        public bool SetAnswer(Viking viking, int qId, int aId)
        {
            // check if answer is in the database already, edit it with new answer id if it does
            modoff.Model.ProfileAnswer? existingAnswer = viking.ProfileAnswers.FirstOrDefault(e => e.QuestionID == qId);
            if(existingAnswer != null)
            {
                existingAnswer.AnswerID = aId;
                ctx.SaveChanges();
                return true;
            }

            // create an answer and store it in database

            modoff.Model.ProfileAnswer answer = new modoff.Model.ProfileAnswer
            {
                AnswerID = aId,
                QuestionID = qId,
            };

            viking.ProfileAnswers.Add(answer);
            ctx.SaveChanges();

            return true;
        }

        public ProfileUserAnswer[] GetUserAnswers(Viking viking)
        {
            // create a profile user answer based on each answer on viking

            List<ProfileUserAnswer> userAnswers = new List<ProfileUserAnswer>();
            foreach(modoff.Model.ProfileAnswer answer in viking.ProfileAnswers)
            {
                ProfileUserAnswer userAnswer = new ProfileUserAnswer
                {
                    AnswerID = answer.AnswerID,
                    QuestionID = answer.QuestionID
                };
                userAnswers.Add(userAnswer);
            }

            return userAnswers.ToArray();
        }

        public ProfileUserAnswer GetUserAnswerFromQuestionId(Viking viking, int qId)
        {
            // check if answer exists
            modoff.Model.ProfileAnswer profileAnswer = viking.ProfileAnswers.FirstOrDefault(e => e.QuestionID == qId);

            if(profileAnswer != null)
            {
                ProfileUserAnswer profileUserAnswer = new ProfileUserAnswer
                {
                    QuestionID = profileAnswer.QuestionID,
                    AnswerID = profileAnswer.AnswerID
                };
                return profileUserAnswer;
            }

            return null;
        }

        public ProfileQuestion GetQuestionFromAnswerId(int aId)
        {
            ProfileAnswer? profileAnswer = allAnswersFromData.FirstOrDefault(e => e.ID == aId);

            if (profileAnswer != null)
            {
                ProfileQuestion? questionFromAnswer = allQuestionsFromData.FirstOrDefault(e => e.ID == profileAnswer.QuestionID);
                return questionFromAnswer!;
            }

            return null!;
        }
    }
}
