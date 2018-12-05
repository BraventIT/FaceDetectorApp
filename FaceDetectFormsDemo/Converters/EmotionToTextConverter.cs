using System;
using System.Globalization;
using FaceDetectFormsDemo.Models;
using Xamarin.Forms;

namespace FaceDetectFormsDemo.Converters
{
    public class EmotionToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var emotion = value as Emotion;
            var emotionEnum = GetEmotion(emotion);
            switch (emotionEnum)
            {
                case EmotionEnum.Anger:
                    return "Emoción: Enfadado";
                case EmotionEnum.Contempt:
                    return "Emoción: Desprecio";
                case EmotionEnum.Disgust:
                    return "Emoción: Disgustado";
                case EmotionEnum.Fear:
                    return "Emoción: Temor";
                case EmotionEnum.Happiness:
                    return "Emoción: Feliz";
                case EmotionEnum.Neutral:
                    return "Emoción: Neutral";
                case EmotionEnum.Sadness:
                    return "Emoción: Triste";
                case EmotionEnum.Surprise:
                    return "Emoción: Sorpresa";

            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        EmotionEnum GetEmotion(Emotion emotion)
        {
            EmotionEnum maxEmotionEnum = EmotionEnum.Anger;
            double maxEmotionValue = 0.0;
            if (emotion.Contempt > maxEmotionValue)
            {
                maxEmotionValue = emotion.Contempt;
                maxEmotionEnum = EmotionEnum.Contempt;
            }
            if (emotion.Disgust > maxEmotionValue)
            {
                maxEmotionValue = emotion.Disgust;
                maxEmotionEnum = EmotionEnum.Disgust;
            }
            if (emotion.Fear > maxEmotionValue)
            {
                maxEmotionValue = emotion.Fear;
                maxEmotionEnum = EmotionEnum.Fear;
            }

            if (emotion.Happiness > maxEmotionValue)
            {
                maxEmotionValue = emotion.Happiness;
                maxEmotionEnum = EmotionEnum.Happiness;
            }

            if (emotion.Neutral > maxEmotionValue)
            {
                maxEmotionValue = emotion.Neutral;
                maxEmotionEnum = EmotionEnum.Neutral;
            }

            if (emotion.Sadness > maxEmotionValue)
            {
                maxEmotionValue = emotion.Sadness;
                maxEmotionEnum = EmotionEnum.Sadness;
            }

            if (emotion.Surprise > maxEmotionValue)
            {
                maxEmotionValue = emotion.Surprise;
                maxEmotionEnum = EmotionEnum.Surprise;
            }
            return maxEmotionEnum;
        }
    }
}
