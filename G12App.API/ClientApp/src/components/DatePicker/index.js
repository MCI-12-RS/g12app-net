import React, { useState } from "react";
import DatePicker from "react-datepicker";
import 'react-datepicker/dist/react-datepicker.css'

function DatePicker() {
  const [date, setDate] = useState(null);
  return (
    <div>
      <DatePicker 
      selected={date} 
      onChange={date => setDate(date)} />
    </div>
  )
}

export default DatePicker;