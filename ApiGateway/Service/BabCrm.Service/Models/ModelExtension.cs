using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabCrm.Service.Models
{
    public static class ModelExtension
    {
        public static CustomerMessageViewModel ToModel(this CustomerMessageModel customerMessage, CultureInfo cultureInfo)
        {
            return new CustomerMessageViewModel()
            {
                ReferenceId = customerMessage.ReferenceId,
                ReplyMessage = customerMessage.ReplyMessage,
                StatusCode = customerMessage.Status,
                Status = customerMessage.StatusName[cultureInfo].Value,
                CreatedOn = customerMessage.CreatedOn,
                CustomerId = customerMessage.CustomerId,
                Subject = customerMessage.Subject,
                Body = customerMessage.Body,
                CustomerFullName = customerMessage.CustomerFullName,
                CustomerMessageSubject = customerMessage.CustomerMessageSubject[cultureInfo].Value,
                CustomerMessageType = customerMessage.CustomerMessageType[cultureInfo].Value,
                CustomerMessageId = customerMessage.CustomerMessageId
            };
        }

        public static CustomerMessageDetailsModel ToModel(this CustomerMessageDetails customerMessage, CultureInfo cultureInfo)
        {
            return new CustomerMessageDetailsModel()
            {
                ReferenceId = customerMessage.ReferenceId,
                ReplyMessage = customerMessage.ReplyMessage,
                CustomerId = customerMessage.CustomerId,
                Subject = customerMessage.Subject,
                Body = customerMessage.Body,
                CustomerFullName = customerMessage.CustomerFullName,
                CustomerMessageSubject = customerMessage.CustomerMessageSubject[cultureInfo].Value,
                CustomerMessageType = customerMessage.CustomerMessageType[cultureInfo].Value,
                CustomerMessageId = customerMessage.CustomerMessageId,
                ConvertedToCaseBy = customerMessage.ConvertedToCaseBy,
                ConvertedToCaseByUserId = customerMessage.ConvertedToCaseByUserId,
                ConvertedToCaseDate = customerMessage.ConvertedToCaseDate,
                ReadBy = customerMessage.ReadBy,
                ReadByUserId = customerMessage.ReadByUserId,
                ReadDate = customerMessage.ReadDate,
                RepliedBy = customerMessage.RepliedBy,
                RepliedByUserId = customerMessage.RepliedByUserId,
                ReplyDate = customerMessage.ReplyDate,
                Status = customerMessage.Status,
                CreatedOn = customerMessage.CreatedOn,
            };
        }

        public static SurveyAnswerViewModel ToModel(this SurveyAnswerModel surveyAnswer, CultureInfo cultureInfo)
        {
            return new SurveyAnswerViewModel()
            {
                Code = surveyAnswer.Code,
                Id = surveyAnswer.Id,
                Name = surveyAnswer.Name[cultureInfo].Value,
                Order = surveyAnswer.Order,
                QuestionId = surveyAnswer.QuestionId,
                Score = surveyAnswer.Score,
            };
        }

        public static SurveyQuestionViewModel ToModel(this SurveyQuestionModel surveyQuestion, CultureInfo cultureInfo)
        {
            return new SurveyQuestionViewModel()
            {
                Code = surveyQuestion.Code,
                Id = surveyQuestion.Id,
                Order = surveyQuestion.Order,
                SurveyTemplateId = surveyQuestion.SurveyTemplateId,
                Weight = surveyQuestion.Weight,
                Name = surveyQuestion.Name[cultureInfo].Value,
            };
        }

        public static SurveyTemplateViewModel ToModel(this SurveyTemplateModel surveyQuestion, CultureInfo cultureInfo)
        {
            return new SurveyTemplateViewModel()
            {
                Descripton = surveyQuestion.Descripton,
                Code = surveyQuestion.Code,
                Id = surveyQuestion.Id,
                Name = surveyQuestion.Name[cultureInfo].Value
            };
        }

    }
}
